using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WiredBrain.Hubs;
using WiredBrain.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WiredBrain.Controllers
{
    [Route("[controller]")]
    public class CoffeeController : Controller
    {
        private readonly IHubContext<CoffeeHub> _coffeeHub;

        public CoffeeController(IHubContext<CoffeeHub> coffeeHub)
        {
            _coffeeHub = coffeeHub;
        }

        [Route("Type")]
        [HttpGet]
        public IActionResult Index()
        {
            return new JsonResult("Americano");
        }

        [HttpPost]
        public async Task<IActionResult> OrderCoffee([FromBody] Order order)
        {
            await _coffeeHub.Clients.All.SendAsync("NewOrder", order);
            return Accepted(Guid.NewGuid());
        }
    }
}

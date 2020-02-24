using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WiredBrain.Helpers;
using WiredBrain.Models;
using System.Text.Json;


namespace WiredBrain.Hubs
{
    public class CoffeeHub: Hub
    {
        private readonly OrderChecker _orderChecker;
        private static HashSet<string> CurrentConnections = new HashSet<string>();

        public CoffeeHub(OrderChecker orderChecker)
        {
            _orderChecker = orderChecker;
        }

        public async Task GetUpdatesForOrder(string orderId)
        {
            CheckResult result;
            var id = Guid.Parse(JsonSerializer.Deserialize<string>(orderId));
            do
            {
                result = _orderChecker.GetUpdate(id);
                Thread.Sleep(1000);

                if (result.New)
                {
                    await Clients.Caller.SendAsync("ReceiveOrderUpdate", result.Update);
                }

            } while (!result.Finished);
            await Clients.Caller.SendAsync("Finished");
        }


        public override async Task OnConnectedAsync()
        {
            var id = Context.ConnectionId;
            CurrentConnections.Add(id);
            await Clients.All.SendAsync("GetCurrentConnections", CurrentConnections.ToList());
            //return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connection = CurrentConnections.FirstOrDefault(x => x == Context.ConnectionId);
            if (connection != null)
            {
                CurrentConnections.Remove(connection);
            }
            await Clients.All.SendAsync("GetCurrentConnections", CurrentConnections.ToList());
            //return base.OnDisconnectedAsync(exception);
        }

        //public async Task GetAllActiveConnections()
        //{
        //    await Clients.All.SendAsync("GetCurrentConnections", CurrentConnections.ToList());
        //    return CurrentConnections.ToList();
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WiredBrain.Helpers
{
    public static class OrdersList
    {
        private static List<Guid> OrdersIds { get; set; } = new List<Guid>();

        public static void AddOrder(Guid clientId)
        {
            OrdersIds.Add(clientId);
        }

        public static List<Guid> GetAllOrders(Guid clientId)
        {
            return OrdersIds;
        }
    }
}

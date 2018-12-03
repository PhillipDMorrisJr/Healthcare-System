using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class TestOrderManager
    {
        public static List<Order> Orders = TestOrderDAL.GetTestOrders();

        public static Order CurrentTestOrder;

        public static List<Order> GetRefreshedOrders()
        {
            Orders = TestOrderDAL.GetTestOrders();
            return Orders;
        }
    }
}
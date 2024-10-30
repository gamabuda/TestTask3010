using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Data.Implementation;

namespace TestTask.Data
{
    internal interface IOrderFilter
    {
        public List<Order> FilterOrders(List<Order> orders, string district, DateTime firstOrderTime);
    }
}

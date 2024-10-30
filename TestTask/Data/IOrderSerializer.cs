using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Data.Implementation;

namespace TestTask.Data
{
    internal interface IOrderSerializer
    {
        public  string FilePath { get; }
        public Task WriteOrdersToFileAsync(List<Order> orders);
        public Task WriteNewOrdersToFileAsync(List<Order> orders, string filepath);
        public Task<List<Order>> ReadOrdersFromFileAsync();
    }
}

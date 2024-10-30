using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Data.Implementation
{
    internal class Order
    {
        public string OrderID { get; set; }

        public double Weight { get; set; }

        public string District { get; set; }

        public DateTime OrderTime { get; set; }

        public Order(double weight, string district, DateTime orderTime)
        {
            OrderID = Guid.NewGuid().ToString();
            Weight = weight;
            District = district;
            OrderTime = orderTime;
        }

        public override string ToString()
        {
            return $"Заказ {OrderID}: Вес = {Weight} кг, Район = {District}, Время = {OrderTime}";
        }
    }
}

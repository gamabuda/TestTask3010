using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestTask.Data;

namespace TestTask.Data.Implementation
{
    internal class OrderServiceStub : IOrderSerializer, IOrderFilter
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "orders.json");
        public string FilePath => _filePath;

        public OrderServiceStub()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine($"Файл создан по умолчанию. Путь: {FilePath}");

                var json = JsonSerializer.Serialize(CreateDefaultList(), new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "orders.json"), json);
            }
        }

        public async Task WriteOrdersToFileAsync(List<Order> orders)
        {
            var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "filterResult.json"), json);
        }

        public async Task WriteNewOrdersToFileAsync(List<Order> orders, string filepath)
        {
            if (!filepath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                filepath += ".json"; 
            }

            var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filepath, json);
        }

        public async Task<List<Order>> ReadOrdersFromFileAsync()
        {
            if (!File.Exists(FilePath))
            {                
                return new List<Order>();
            }

            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Order>>(json);
        }

        public List<Order> FilterOrders(List<Order> orders, string district, DateTime firstOrderTime)
        {
            // Устанавливаем верхнюю границу по времени (30 минут после первого заказа)
            DateTime upperTimeLimit = firstOrderTime.AddMinutes(30);

            var filteredOrders = orders
                .Where(o => o.District.Equals(district, StringComparison.OrdinalIgnoreCase) &&
                            o.OrderTime >= firstOrderTime &&
                            o.OrderTime <= upperTimeLimit)
                .ToList();

            return filteredOrders;
        }

        private List<Order> CreateDefaultList()
        {
            List<Order> orders = new List<Order>();
            Random random = new Random();

            string[] locations =
            {
            "Московский", "Питерский", "Казанский",
            "Екатеринбургский", "Нижненовгородский",
            "Сибирский", "Уральский", "Южный"
        };

            for (int i = 0; i < 12; i++)
            {
                double amount = Math.Round(random.NextDouble() * 10, 2); 
                string location = locations[random.Next(locations.Length)]; 
                DateTime orderDate = DateTime.Now.AddDays(random.Next(0, 30)); 

                orders.Add(new Order(amount, location, orderDate));
            }

            return orders;
        }
    }
}

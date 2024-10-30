using System;
using System.IO;
using TestTask;
using TestTask.Data;
using TestTask.Data.Implementation;

string _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");

var _orderServiceStub = new OrderServiceStub();
var _orders = await _orderServiceStub.ReadOrdersFromFileAsync();

Log("Запущена программа.");
Console.WriteLine($"Тестовое задание для Effective Mobile.\nДля получения списка доступных вам команд воспользуйтесь командой help\n");

while (true)
{
    string input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Пожалуйста, введите команду.");
        continue;
    }

    string[] parts = input.Split(' ');
    string command = parts[0];

    switch (command)
    {
        case "help":
            Log("Команда 'help' выполнена");
            Console.WriteLine("\nsave | сохранение текущего списка заказов в файл filterResult\nprint | вывод списка заказов\nfilter {район} {дд.мм.гггг чч:мм} | фильтрация списка зазазов по району и времени\nq | завершение работы программы\n");
            break;

        case "filter":
            if (parts.Length < 3)
            {
                Console.WriteLine("Ошибка: недостаточно аргументов для команды: filter {район} {дд.мм.гггг чч:мм}");
                Log("Ошибка: недостаточно аргументов для команды 'filter'");
                break;
            }
            string district = parts[1];
            string dateTimeString = string.Join(" ", parts, 2, parts.Length - 2).Trim();

            if (DateTime.TryParseExact(dateTimeString, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime orderTime))
            {
                _orders = _orderServiceStub.FilterOrders(_orders, district, orderTime);
                Log($"Команда 'filter' выполнена с параметрами: district = {district}, orderTime = {orderTime}");

                foreach (var item in _orders)
                    Console.WriteLine(item);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный формат даты и времени. Ожидается формат дд.мм.гггг чч:мм.");
                Log("Ошибка: неверный формат даты и времени для команды 'filter'");
            }
            break;

        case "print":
            if (_orders.Count == 0)
            {
                Log("Команда 'print' выполнена: список заказов пуст.");
                Console.WriteLine("Список пуст.");
                return;
            }

            foreach (var item in _orders)
                Console.WriteLine(item);

            Log("Команда 'print' выполнена.");
            break;

        case "save":
            Log("Команда 'save' выполнена: заказы сохранены.");
            await _orderServiceStub.WriteOrdersToFileAsync(_orders);
            break;

        case "q":
            Log("Программа завершена пользователем.");
            return;

        default:
            Log($"Неизвестная команда: {command}");
            Console.WriteLine("Неизвестная команда.");
            break;
    }
}


void Log(string message)
{
    using (StreamWriter writer = new StreamWriter(_logFilePath, true))
    {
        writer.WriteLine($"{DateTime.Now}: {message}");
    }
}

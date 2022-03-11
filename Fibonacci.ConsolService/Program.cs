using System;
using System.Net.Http;
using EasyNetQ;
using Fibonacci.Abstract;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    static class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        static void Main(string[] args)
        {
            Client.BaseAddress = new Uri("https://localhost:44350");
            using var bus = RabbitHutch.CreateBus("host=localhost");
            Console.WriteLine("Listening for messages.");
            bus.PubSub.Subscribe<FibMessage>("test1", Handler);
            Console.ReadLine();
        }

        /// <summary>
        /// Хендлер для обработки сообщения через шину сообщений.
        /// </summary>
        /// <param name="textMessage">Полученное сообщение <see cref="FibMessage"/>.</param>
        private static void Handler(FibMessage textMessage)
        {
            Console.WriteLine($"Message received: {textMessage.Number} ");
            if(textMessage.Number == 0)
                return;
            Console.WriteLine($"WebApi request with n: {textMessage.Number-1} ");
            var result = Client.GetFibonacci(textMessage.Number - 1);
            Console.WriteLine($"WebApi response: {result} ");
        }

        /// <summary>
        /// Запрос к "/api/Fibonacci/FibonacciInstance".
        /// </summary>
        /// <param name="client">Http клиент.</param>
        /// <param name="number">Число для рассчета.</param>
        /// <returns>Полученное от сервиса число.</returns>
        private static int GetFibonacci(this HttpClient client, int number)
        {
            var result = client.GetAsync($"/api/Fibonacci/FibonacciInstance?n={number}").Result;
            return JsonConvert.DeserializeObject<FibMessage>(result.Content.ReadAsStringAsync().Result).Number;
        }
    }
}

using System;
using System.Threading.Tasks;
using Fibonacci.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Fibonacci.ApiService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FibonacciController : Microsoft.AspNetCore.Mvc.Controller
    {
        private IRabbitAdapter _messageBus;
        //private readonly IBus _bus;
        
        public FibonacciController(IRabbitAdapter messageBus)
        {
            var user = Environment.UserName;
            _messageBus = messageBus;
            //_bus = messageBus;
        }

        [HttpGet(nameof(FibonacciInstance))]
        public IActionResult FibonacciInstance(int n)
        {
            var fib1 = Task.Run(() => Fibonacci(n));
            var fib2 = Task.Run(() => Fibonacci(n - 1));

            //Вариант через напрямую с RabbitHutch
            //using var bus = RabbitHutch.CreateBus("host=localhost");
            //bus.PubSub.Publish(new FibMessage() { Number = fib1.Result + fib2.Result });

            //Вариант через напрямую с IRabbitAdapter
            _messageBus.SendMessage(fib1.Result + fib2.Result);

            return Json(new FibMessage() { Number = n });
        }

        static int Fibonacci(int n)
        {
            if (n == 0 || n == 1) return n;

            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        static Task<int> FibonacciTask(int n)
        {
            return new Task<int>(() => Fibonacci(n));
        }
    }
}

using System;
using EasyNetQ;
using Fibonacci.Abstract;
using FibMessage = Fibonacci.Abstract.FibMessage;

namespace Fibonacci.Core
{
    /// <summary>
    /// Адаптер для шины сообщений.
    /// </summary>
    public class RabbitAdapter : IRabbitAdapter
    {
        /// <summary>
        /// Шина сообщений.
        /// </summary>
        private readonly IBus _messageBus;

        /// <summary>
        /// Флаг очистки.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connection"></param>
        public RabbitAdapter(string connection)
        {
            _messageBus = RabbitHutch.CreateBus(connection);
        }


        /// <inheritdoc />
        public void SendMessage(int message)
        {
            _messageBus.PubSub.Publish(
                new FibMessage()
                {
                    Number = message
                }
            );
        }

        /// <inheritdoc />
        public void ReceiveMessage(Action<IFibMessage> handler)
        {
            _messageBus.PubSub.Subscribe<FibMessage>("test", handler);
        }

        /// <summary>
        /// Утилизация.
        /// </summary>
        /// <param name="disposing">Флаг утилизации.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _messageBus.Dispose();
                }
            }

            _disposed = true;
        }

        /// <summary>
        /// Утилизация.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

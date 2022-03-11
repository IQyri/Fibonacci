using System;

namespace Fibonacci.Abstract
{
    /// <summary>
    /// Интерфейс адаптера шины сообщений для расчета последовательности фибоначи
    /// </summary>
    public interface IRabbitAdapter : IDisposable
    {
        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="message">Числовое сообщение.</param>
        public void SendMessage(int message);
        /// <summary>
        /// Подписка на сообщения типа <see cref="IFibMessage"/>
        /// </summary>
        /// <param name="handler">Обработчик сообщений.</param>
        public void ReceiveMessage(Action<IFibMessage> handler);
    }
}

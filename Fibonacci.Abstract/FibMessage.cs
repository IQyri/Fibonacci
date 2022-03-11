namespace Fibonacci.Abstract
{
    /// <summary>
    /// Сообщение для шины.
    /// </summary>
    public class FibMessage : IFibMessage
    {
        /// <summary>
        /// Число для расчетов.
        /// </summary>
        public int Number { get; set; }
    }
}

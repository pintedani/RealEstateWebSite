namespace Imobiliare.Repositories
{
    public class PropertyChange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChange"/> class.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="currentValue">The current value.</param>
        public PropertyChange(object originalValue, object currentValue)
        {
            this.OriginalValue = originalValue;
            this.CurrentValue = currentValue;
        }

        /// <summary>
        /// Gets the original value.
        /// </summary>
        public object OriginalValue { get; }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public object CurrentValue { get; }
    }
}
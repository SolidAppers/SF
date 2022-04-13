namespace SF.Core.Utilities.Comparators
{
    internal class Comparator
    {
        internal virtual bool AreEqual(object value1, object value2)
        {
            return Equals(value1, value2);
        }
    }
}

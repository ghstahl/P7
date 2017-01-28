namespace P7.BlogStore.Hugo
{
    static class ConcurrencyLock
    {
        private static object _theLock;
        public static object TheLock
        {
            get { return _theLock ?? (_theLock = new object()); }
        }
    }
}
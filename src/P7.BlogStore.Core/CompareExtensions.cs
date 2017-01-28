namespace P7.BlogStore.Core
{
    public static class CompareExtensions
    {
        public static bool IsEqual(this object a, object b)
        {
            if (a != null && b != null)
            {
                if (!a.Equals(b))
                    return false;
            }

            if (!(a == null && b == null))
            {
                return false;
            }
            return true;
        }
    }
}
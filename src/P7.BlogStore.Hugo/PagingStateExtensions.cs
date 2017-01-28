using System.IO;
using BinaryFormatter;

namespace P7.BlogStore.Hugo
{
    static public class PagingStateExtensions
    {
        public static byte[] Serialize(this PagingState pagingState)
        {
            if (pagingState == null)
                return null;
            BinaryConverter converter = new BinaryConverter();
            byte[] byteArray = converter.Serialize(pagingState);
            return byteArray;
        }
        public static PagingState Deserialize(this byte[] bytes)
        {
            if (bytes == null)
                return new PagingState() { CurrentIndex = 0 };

            BinaryConverter converter = new BinaryConverter();
            PagingState ps = converter.Deserialize<PagingState>(bytes);
            return ps;
        }
    }
}
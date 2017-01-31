using System.IO;
using BinaryFormatter;
using ProtoBuf;

namespace P7.Store
{
    static public class PagingStateExtensions
    {
        public static byte[] Serialize(this PagingState pagingState)
        {
            if (pagingState == null)
                return null;
            byte[] byteArray = null;
            using (var memoryResponse = new MemoryStream())
            {
                Serializer.Serialize(memoryResponse, pagingState);
                byteArray = memoryResponse.ToArray();
            }
            return byteArray;
        }

        public static PagingState Deserialize(this byte[] bytes)
        {
            if (bytes == null)
                return new PagingState() {CurrentIndex = 0};
            PagingState pagingState = null;
            using (var memoryResponse = new MemoryStream(bytes))
            {
                pagingState = Serializer.Deserialize<PagingState>(memoryResponse);
            }
            return pagingState;
        }
    }
}
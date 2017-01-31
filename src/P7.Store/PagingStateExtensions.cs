﻿using System;
using System.IO;
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
        public static string SerializeToBase64String(this PagingState pagingState)
        {
            if (pagingState == null)
                return null;
            byte[] bytes = pagingState.Serialize();
            var psString = Convert.ToBase64String(bytes);
            return psString;
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
        public static PagingState DeserializeFromBase64String(this string psString)
        {
            if (string.IsNullOrEmpty(psString))
                return new PagingState() { CurrentIndex = 0 };
            var bytes = Convert.FromBase64String(psString);
            PagingState pagingState = bytes.Deserialize();
            return pagingState;
        }
    }
}
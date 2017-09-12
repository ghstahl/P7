using System;
using System.Collections.Generic;
using System.Linq;

namespace P7.External.SPA.Core
{
    public static class ListExtensions
    {
        public static void Replace<T>(this List<T> list, Predicate<T> oldItemSelector, T newItem)
        {
            //check for different situations here and throw exception
            //if list contains multiple items that match the predicate
            //or check for nullability of list and etc ...
            var oldItemIndex = list.FindIndex(oldItemSelector);
            list[oldItemIndex] = newItem;
        }
    }
    public class InMemoryExternalSpaStore: IExternalSPAStore
    {
        private Dictionary<string,ExternalSPARecord> _records;

        private Dictionary<string, ExternalSPARecord> Records
        {
            get => _records ?? (_records = new Dictionary<string, ExternalSPARecord>());
            set => _records = value;
        }

        public ExternalSPARecord GetRecord(string key)
        {
            var sKey = key.ToLower();
            if (Records.ContainsKey(sKey))
            {
                return Records[sKey];
            }
            return null;
        }

        public void AddRecord(ExternalSPARecord record)
        {
            var sKey = record.Key.ToLower();

            Records[sKey] = record;
        }

        public void RemoveRecord(string key)
        {
            var sKey = key.ToLower();
            if (Records.ContainsKey(sKey))
            {
                Records.Remove(sKey);
            }
        }

        public IEnumerable<ExternalSPARecord> GetRecords()
        {
            var query = from item in Records
                let c = item.Value
                select c;
            return query;
        }
    }
}

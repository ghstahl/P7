using System;
using System.Collections.Generic;
using System.Text;

namespace P7.External.SPA.Core
{
    public class ExternalSPARecord
    {
        public string Key { get; set; }
        public bool RequiredAuth { get; set; }
        public string RenderTemplate { get; set; }
    }
    public interface IExternalSPAStore
    {
        ExternalSPARecord GetRecord(string key);
        void AddRecord(ExternalSPARecord record);
        void RemoveRecord(string key);
        IEnumerable<ExternalSPARecord> GetRecords();
    }
}

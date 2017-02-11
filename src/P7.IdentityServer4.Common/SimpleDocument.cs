using Newtonsoft.Json;

namespace P7.IdentityServer4.Common
{
    public class SimpleDocument<T> : ISimpleDocument where T : class
    {
        private readonly T _document;

        public SimpleDocument(T document)
        {
            _document = document;
        }

        public SimpleDocument(string documentJson)
        {
            _document = JsonConvert.DeserializeObject<T>(documentJson);
        }

        public object Document => _document;

        public string DocumentJson
        {
            get
            {
                string output = JsonConvert.SerializeObject(_document);
                return output;
            }
        }
    }
}
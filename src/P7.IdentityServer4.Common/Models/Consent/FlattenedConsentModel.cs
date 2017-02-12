using System.Collections.Generic;

namespace P7.IdentityServer4.Common.Models.Consent
{
    public class FlattenedConsentModel :
        AbstractConsentHandle<string>
    {
        public FlattenedConsentModel()
            : base()
        {
        }

        public FlattenedConsentModel(global::IdentityServer4.Models.Consent consent) : base(consent)
        {
        }

        public override string Serialize(List<string> scopes)
        {
            if (scopes == null)
                return "[]";
            var simpleDocument = new SimpleDocument<List<string>>(scopes).DocumentJson;
            return simpleDocument;
        }

        public override List<string> DeserializeScopes(string obj)
        {
            obj = string.IsNullOrEmpty(obj) ? "[]" : obj;
            var simpleDocument = new SimpleDocument<List<string>>(obj);
            var document = (List<string>)simpleDocument.Document;
            return document;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class KeyValueContainer {
        private List<KeyValue> keyValues = new List<KeyValue>();
        public void Add(string key, string value) {
            if (key.Equals("id")) {
                throw new ArgumentException("id wird nicht mittels" +
                    "key value Paaren bearbeitet");
            }
            keyValues.Add(new KeyValue(key, value));
        }
        public string GetValue(string key) {
            foreach (KeyValue keyValuePair in keyValues) {
                if (keyValuePair.GetKey().Equals(key)) {
                    return keyValuePair.GetValue();
                }
            }
            throw new ArgumentException("key existiert nicht");
        }

        public List<KeyValue> GetList() {
            return keyValues;
        }
    }
}

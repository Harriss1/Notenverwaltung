using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    /// <summary>
    /// Beschreibt die Attribute und deren Werte einer Entität
    /// Beschreibt seperat den PrimaryKey und dessen Wert
    /// </summary>
    internal class AttributeToValuesDescription {
        public int primaryKeyValue { get; private set; }
        public string primaryKey { get; private set; }
        public AttributeToValuesDescription(string primaryKey, int primaryKeyValue) {
            this.primaryKey = primaryKey;
            this.primaryKeyValue = primaryKeyValue;
        }
        private AttributeToValuesDescription() {
            // Standard-Konstruktor soll nicht verwendet werden
        }
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
            throw new ArgumentException("key ('" + key + "') existiert nicht");
        }

        /// <summary>
        /// Alle Attribute bis auf primaryKey-Column als Liste ausgeben
        /// </summary>
        /// <returns></returns>
        public List<KeyValue> GetList() {
            return keyValues;
        }
    }
}

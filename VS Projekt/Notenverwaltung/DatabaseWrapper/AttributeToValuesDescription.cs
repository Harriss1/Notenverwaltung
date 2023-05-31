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
        private List<KeyValue> keyValues = new List<KeyValue>();
        private List<KeyValue> relationships = new List<KeyValue>();
        public AttributeToValuesDescription(string primaryKey, int primaryKeyValue) {
            this.primaryKey = primaryKey;
            this.primaryKeyValue = primaryKeyValue;
        }
        private AttributeToValuesDescription() {
            // Standard-Konstruktor soll nicht verwendet werden
        }
        public void AddStringAttribute(string key, string value) {
            if (key.Equals("id")) {
                throw new ArgumentException("id wird nicht mittels" +
                    "key value Paaren bearbeitet");
            }
            keyValues.Add(new KeyValue(key, value));
        }
        public void AddRelation(string key, int foreignKey) {
            if (key.Equals("id")) {
                throw new ArgumentException("id wird nicht mittels" +
                    "key value Paaren bearbeitet");
            }
            relationships.Add(new KeyValue(key, foreignKey));
        }

        public void AddDateTimeAttribute(string key, DateTime date) {
            keyValues.Add(new KeyValue(key, date));
        }

        public int GetRelationId(string key) {
            if (key.Equals("id")) {
                throw new ArgumentException("id wird nicht mittels" +
                    "key value Paaren bearbeitet");
            }
            foreach (KeyValue keyValuePair in relationships) {
                if (keyValuePair.GetKey().Equals(key)) {
                    return (int)keyValuePair.GetValue();
                }
            }
            return -1;
        }

        public object GetValue(string key) {
            foreach (KeyValue keyValuePair in keyValues) {
                if (keyValuePair.GetKey().Equals(key)) {
                    return keyValuePair.GetValue();
                }
            }
            throw new ArgumentException("key ('" + key + "') existiert nicht");
        }

        /// <summary>
        /// Alle Attribute bis auf primaryKey-Column und Beziehungen als Liste ausgeben
        /// </summary>
        /// <returns></returns>
        public List<KeyValue> GetAttributes() {
            return keyValues;
        }
        public List<KeyValue> GetRelations() {
            return relationships;
        }
    }
}

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
        public int primaryKeyValue;
        public string primaryKey { get; private set; }
        private List<KeyValue> keyValues = new List<KeyValue>();
        private List<KeyValue> singleRelationships = new List<KeyValue>();
        private List<OneToXRelationKeyValue> oneToXRelationships = new List<OneToXRelationKeyValue>();
        private List<ManyToManyKeyValue> manyToManyRelationships = new List<ManyToManyKeyValue>();
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

        internal void AddOneToXRelation(OneToXRelationKeyValue toOneKeyValue) {
            if (GetOneToXRelation(toOneKeyValue.GetForeignTable()) != null) {
                throw new ArgumentException("Beziehung wurde bereits definiert für Tabelle=" + toOneKeyValue.GetForeignTable());
            }
            oneToXRelationships.Add(toOneKeyValue);
        }
        public List<OneToXRelationKeyValue> GetOneToXRelations() {
            return oneToXRelationships;
        }

        internal OneToXRelationKeyValue GetOneToXRelation(string foreignTablename) {
            foreach (OneToXRelationKeyValue relationDescription in oneToXRelationships) {
                if (relationDescription.GetForeignTable().Equals(foreignTablename)) {
                    return relationDescription;
                }
            }
            return null;
        }

        [Obsolete]
        public void AddRelation(string key, int foreignKey) {
            if (key.Equals("id")) {
                throw new ArgumentException("id wird nicht mittels" +
                    "key value Paaren bearbeitet");
            }
            singleRelationships.Add(new KeyValue(key, foreignKey));
        }

        public void AddDateTimeAttribute(string key, DateTime date) {
            keyValues.Add(new KeyValue(key, date));
        }
        internal void AddIntegerAttribute(string key, int number) {
            keyValues.Add(new KeyValue(key, number));
        }
        [Obsolete]
        public int GetRelationId(string key) {
            if (key.Equals("id")) {
                throw new ArgumentException("id wird nicht mittels" +
                    "key value Paaren bearbeitet");
            }
            foreach (KeyValue keyValuePair in singleRelationships) {
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
        [Obsolete]
        public List<KeyValue> GetRelations() {
            return singleRelationships;
        }

        public void AddManyToManyRelation(ManyToManyKeyValue manyToManyKeyValue) {
            manyToManyRelationships.Add(manyToManyKeyValue);
        }
        public object GetManyToManyRelation(string tableName) {
            foreach (ManyToManyKeyValue relation in manyToManyRelationships) {
                if (relation.GetTablename().Equals(tableName)) {
                    return relation;
                }
            }
            return null;
        }
        public List<ManyToManyKeyValue> GetAllManyToManyRelations() {
            return manyToManyRelationships;
        }

    }
}

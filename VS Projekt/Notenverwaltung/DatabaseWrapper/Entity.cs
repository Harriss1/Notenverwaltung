using System.Collections.Generic;

namespace Notenverwaltung {
    internal abstract class Entity {
        private DatabaseStorage storage;

        // Primary Key
        public int id { get; protected set; }

        public Entity() {
            id = -1;
            storage = (DatabaseStorage)GlobalObjects.Get(InterfaceListing.DatabaseStorage);
        }

        public abstract string ToTableName();

        /// <summary>
        /// Mapping der Objekt-Attribute hin zu Tabellen-Spalten
        /// Id bzw. Primary-Key wird separat mittels ToPrimaryKeyColumnName() gemappt
        /// </summary>
        /// <returns></returns>
        public abstract AttributeToValuesDescription ToAttributeValueDescription();

        /// <summary>
        /// Alle Attribute der Entität mittels einer Key-Value Liste setzen.
        /// 
        /// Vorsicht: Nicht verwechseln mit Aktualisierung eines Datensatzes.
        /// </summary>
        /// <param name="attributeToValuesDescription">Beschreibung der Entität Attribut-Zu-Attributwerte</param>
        protected abstract void SetAttributesFromInternal(AttributeToValuesDescription attributeToValuesDescription);

        public Entity Create() {
            int lastRowId = storage.InsertSingleEntity(this);
            this.id = lastRowId;
            return this;
        }

        public Entity Update() {
            storage.UpdateSingleEntity(this);
            return this;
        }

        public void Delete() {
            storage.Delete(this);
            id = -1;
        }

        public string ToText() {
            string text = "";
            text += "ID=" + this.id;
            text += " | ";

            for (int i = 0; i < ToAttributeValueDescription().GetAttributes().Count; i++) {
                text += "" + ToAttributeValueDescription().GetAttributes()[i].GetKey() + "=";
                text += ToAttributeValueDescription().GetAttributes()[i].GetValueString();
                if (i < ToAttributeValueDescription().GetAttributes().Count - 1) {
                    text += " | ";
                }
            }
            for (int i = 0; i < ToAttributeValueDescription().GetRelations().Count; i++) {
                text += "" + ToAttributeValueDescription().GetRelations()[i].GetKey() + "=";
                text += ToAttributeValueDescription().GetRelations()[i].GetValue();
                if (i < ToAttributeValueDescription().GetRelations().Count - 1) {
                    text += " | ";
                }
            }
            text += "";
            return text;
        }
        /// <summary>
        /// Konsolenausgabe: Tabellenname + Inhalte der Felder der einzelnen Entität
        /// </summary>
        public void Print() {
            System.Console.WriteLine(this.ToTableName() + ": " + this.ToText());
        }

        public Entity FindFirstByStringAttribute(KeyValue keyValuePair) {
            AttributeToValuesDescription retrievedContent = storage.FindByKeyValue(keyValuePair, this);
            if (retrievedContent == null) {
                // Falls kein Datensatz gefunden wird
                return null;
            }
            SetAttributesFromInternal(retrievedContent);
            return this;
        }
        public Entity FindById(int id) {
            AttributeToValuesDescription retrievedContent = storage.FindById(id, this);
            if (retrievedContent == null) {
                // Falls kein Datensatz gefunden wird
                return null;
            }
            SetAttributesFromInternal(retrievedContent);
            return this;
        }
        public Entity FindFirstByAttributes(List<KeyValue> keyValues) {
            return null;
        }

        public List<Entity> SearchByAttributes(List<KeyValue> keyValues) {
            return null;
        }
        public List<Entity> Search(string key, string value) {
            return null;
        }
    }
}

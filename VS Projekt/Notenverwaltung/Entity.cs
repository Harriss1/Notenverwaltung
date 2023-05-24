using System.Collections.Generic;

namespace Notenverwaltung {
    internal abstract class Entity {
        private DatabaseStorage storage;

        // Primary Key
        public int id { get; protected set; }

        public Entity() {
            storage = (DatabaseStorage)GlobalObjects.Get(InterfaceListing.DatabaseStorage);
        }

        public abstract string ToTableName();
        public abstract string ToPrimaryKeyColumnName();

        /// <summary>
        /// Mapping der Objekt-Attribute hin zu Tabellen-Spalten
        /// Id bzw. Primary-Key wird separat mittels ToPrimaryKeyColumnName() gemappt
        /// </summary>
        /// <returns></returns>
        public abstract List<KeyValue> ToKeyValue();
        public abstract KeyValueContainer ToKeyValueAttributeList();

        /// <summary>
        /// Alle Attributes mittels einer Key-Value Liste füllen.
        /// </summary>
        /// <param name="attributeList">Liste mit key-value Paaren</param>
        public abstract void SetAllAttributes(KeyValueContainer attributeList, int id);

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
            for (int i = 0; i < ToKeyValue().Count; i++) {
                text += "" + ToKeyValue()[i].GetKey() + "=";
                text += ToKeyValue()[i].GetValue();
                if (i < ToKeyValue().Count - 1) {
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

        public Entity FindFirst(string key, string value) {
            return null;
        }
        public Entity FindById(int id) {
            //Entity entity = storage.FindById(id);
            //if (entity != null) {
            //    SelfUpdate(entity);
            //    return this; 
            //}
            // FromKeyValue!!!
            return null;
        }
        private void SelfUpdate(Entity entity) {
            foreach (KeyValue keyValuePair in ToKeyValue()) {
                keyValuePair.GetKey();
            }
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

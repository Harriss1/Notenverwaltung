using System.Collections.Generic;

namespace Notenverwaltung {
    /// <summary>
    /// Elternklasse um eine Entität einer Datenbank zu definieren
    /// </summary>
    internal abstract class Entity {
        private DatabaseStorage storage;
        protected const int recursionMaxDepth = 3;
        /// <summary>
        /// Parameter muss auf Wahr gesetzt werden, falls neue Beziehungen hinzukommen.
        /// </summary>
        protected bool isNewManyToManyRelationAdded = false;

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
        //protected void InokeAttributeSetter(AttributeToValuesDescription attributeToValuesDescription, int recursionLevel) {
        //    SetAttributesFromInternal(attributeToValuesDescription);
        //}

        public Entity Create() {
            int lastRowId = storage.InsertSingleEntity(this);
            this.id = lastRowId;            

            // Mittels FindById werden die Beziehungen der Objekte aktualisiert
            // Dies müsste korrigiert werden, da man es da nicht vermutet
            return FindById(lastRowId);
        }

        public Entity Update() {
            storage.UpdateSingleEntity(this);

            // Many-To-Many-Beziehungen in Datenbank einpflegen, falls vorhanden
            if (this.isNewManyToManyRelationAdded) {
                // Wir können nur Beziehungen hinzufügen, zur Zeit sind
                // keine Beziehungen löschbar
                storage.InsertManyToManyRelationsIfMissing(this);
                isNewManyToManyRelationAdded= false;
            }
            // Mittels FindById werden die Beziehungen der Objekte aktualisiert
            // Dies müsste korrigiert werden, da man es da nicht vermutet
            return FindById(this.id);
        }

        public void Delete() {
            storage.Delete(this);
            id = -1;
        }
        public Entity FindById(int primaryKey) {
            System.Console.WriteLine("FindById id=" + primaryKey);
            if (primaryKey <= -1) {
                throw new System.ArgumentOutOfRangeException("id value=" + primaryKey);
            }
            AttributeToValuesDescription retrievedContent = storage.FindById(primaryKey, this);
            if (retrievedContent == null) {
                // Falls kein Datensatz gefunden wird
                return null;
            }
            retrievedContent.primaryKeyValue = primaryKey;
            SetAttributesFromInternal(retrievedContent);
            return this;
        }
        internal Entity FindById(int primaryKey, int recursionLevel) {
            
            System.Console.WriteLine("FindById id=" + primaryKey);
            if (primaryKey <= -1) {
                throw new System.ArgumentOutOfRangeException("id value=" + primaryKey);
            }
            AttributeToValuesDescription retrievedContent = storage.FindById(primaryKey, this);
            if (retrievedContent == null) {
                // Falls kein Datensatz gefunden wird
                return null;
            }
            retrievedContent.primaryKeyValue = primaryKey;
            retrievedContent.recursionLevel++;
            if (recursionLevel >= recursionMaxDepth) {
                System.Console.WriteLine("Rekursiontiefe erreicht.");
                return null;
            }
            SetAttributesFromInternal(retrievedContent);
            //InokeAttributeSetter(retrievedContent, recursionLevel);
            return this;
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
        public Entity FindFirstByAttributes(List<KeyValue> keyValues) {
            return null;
        }

        public List<Entity> SearchByAttributes(List<KeyValue> keyValues) {
            return null;
        }
        public List<Entity> Search(string key, string value) {
            return null;
        }
        /// <summary>
        /// Konsolenausgabe: Tabellenname + Inhalte der Felder der einzelnen Entität
        /// </summary>
        public void Print() {
            System.Console.WriteLine(this.ToTableName() + ": " + this.ToText());
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
            for (int i = 0; i < ToAttributeValueDescription().GetOneToXRelations().Count; i++) {
                text += " | " + ToAttributeValueDescription().GetOneToXRelations()[i].GetOwnForeignColumnName() + "=";
                text += ToAttributeValueDescription().GetOneToXRelations()[i].GetForeignId();
                if (i < ToAttributeValueDescription().GetOneToXRelations().Count - 1) {
                    text += " | ";
                }
            }
            text += "";
            return text;
        }
    }
}

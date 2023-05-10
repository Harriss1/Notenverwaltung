using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Print() {
            System.Console.WriteLine(this.ToTableName() + ": " + this.ToText());
        }

        public Entity Find(string key, string value) {
            return null;
        }
        public Entity FindById(int id) {
            return null;
        }
    }
}

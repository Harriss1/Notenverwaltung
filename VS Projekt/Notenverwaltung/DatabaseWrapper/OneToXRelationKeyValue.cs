using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class OneToXRelationKeyValue {
        private string foreignTableName;
        private string foreignPrimaryColumnName;
        private int foreignKey; // Ist der Fremdschlüssel zur Herstellen der Verbindung
        private string ownForeignColumnName;
        public OneToXRelationKeyValue(string foreignTableName,
                                    string foreignPrimaryColumnName,
                                    string ownForeignColumnName,
                                    int foreignKey) {
            this.foreignTableName = foreignTableName;
            this.foreignPrimaryColumnName = foreignPrimaryColumnName;
            this.ownForeignColumnName = ownForeignColumnName;
            this.foreignKey = foreignKey;
        }
        private OneToXRelationKeyValue() {
            //Default Konstruktur deaktiviert
        }
        public string GetForeignTable() {
            return foreignTableName;
        }
        public int GetForeignId() {
            return foreignKey;
        }
        public string GetForeignPrimaryColumnName() {
            return foreignPrimaryColumnName;
        }

        internal string GetOwnForeignColumnName() {
            return ownForeignColumnName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class ManyToManyKeyValue {
        private string manyToManyTableName;
        private string ownColumn;
        private string foreignColumn;
        private int foreignId;
        public ManyToManyKeyValue(string manyToManyTableName, string ownColumn, string foreignColumn, int foreignId) {
            this.manyToManyTableName = manyToManyTableName;
            this.ownColumn = ownColumn;
            this.foreignColumn = foreignColumn;
            this.foreignId = foreignId;
        }
        private ManyToManyKeyValue() {
            //Default Konstruktur deaktiviert
        }
        public string GetTable() {
            return manyToManyTableName;
        }
        public int GetForeignId() {
            return foreignId;
        }
        public string GetOwnColumn() {
            return ownColumn;
        }
        public string GetForeignColumn() {
            return foreignColumn;
        }
    }
}

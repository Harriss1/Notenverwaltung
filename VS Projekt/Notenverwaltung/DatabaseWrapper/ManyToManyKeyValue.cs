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
        private List<int> foreignIds;
        public ManyToManyKeyValue(  string manyToManyTableName, 
                                    string ownColumn, 
                                    string foreignColumn, 
                                    List<int> foreignIds) {
            this.manyToManyTableName = manyToManyTableName;
            this.ownColumn = ownColumn;
            this.foreignColumn = foreignColumn;
            this.foreignIds = foreignIds;
        }
        private ManyToManyKeyValue() {
            //Default Konstruktur deaktiviert
        }
        public string GetTablename() {
            return manyToManyTableName;
        }
        public List<int> GetForeignIds() {
            return foreignIds;
        }
        public string GetOwnColumn() {
            return ownColumn;
        }
        public string GetForeignColumn() {
            return foreignColumn;
        }
    }
}

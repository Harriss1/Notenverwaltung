using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Person : Model {
        public string GetName() { return ""; }

        public Person FindFirst(int id) {
            string sql = GetScripts().DropAllTables();
            
            return (Person) GetDb().FindFirstById("person", id);
        }


    }
}

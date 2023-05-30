using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal interface BuilderQueries {

        string[] CreateAllTables();

        string DropAllTables();

        string[] InsertDemoData();

    }
}

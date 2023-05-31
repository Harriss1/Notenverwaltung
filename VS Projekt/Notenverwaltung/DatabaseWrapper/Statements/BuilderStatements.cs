using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    // Dependency Injection Naming-Konvention favorisiert kein "I"-Präfix
#pragma warning disable IDE1006 
    internal interface BuilderStatements {
#pragma warning restore IDE1006

        string[] CreateAllTables();

        string DropAllTables();

        string[] InsertDemoData();

    }
}

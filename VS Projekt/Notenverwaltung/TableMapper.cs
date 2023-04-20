using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal interface TableMapper {
        string ToTableName();
        List<string> ToColumnNames();
    }
}

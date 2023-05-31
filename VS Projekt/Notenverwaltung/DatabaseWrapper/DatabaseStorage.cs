using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    /// <summary>
    /// Zuständig um Datensätze in einer fertigen Datenbank
    /// mittels Create Read Update Delete Methoden zu manipulieren 
    /// </summary>
#pragma warning disable IDE1006 
    // Dependency Injection Naming-Konvention favorisiert kein "I"-Präfix
    internal interface DatabaseStorage {
#pragma warning restore IDE1006
        /// <summary>
        /// Findet einen Lehrer anhand seiner Id in der Tabelle.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        object FindTeacherById(string tableName, int id);
        object FindTeacherByLastname(string tableName, string lastname);

        /// <summary>
        /// Löscht den Datensatz anhand seiner Id
        /// </summary>
        /// <param name="entity"></param>
        void Delete(Entity entity);

        /// <summary>
        /// Erstellt einen Datensatz in einer Tabelle
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Id des primarykey</returns>
        int InsertSingleEntity(Entity entity);

        /// <summary>
        /// Aktualisiert einen Datensatz in einer Tabelle
        /// </summary>
        /// <param name="entity"></param>
        void UpdateSingleEntity(Entity entity);
        AttributeToValuesDescription FindById(int id, Entity entity);
        AttributeToValuesDescription FindByKeyValue(KeyValue keyValuePair, Entity entity);
    }
}

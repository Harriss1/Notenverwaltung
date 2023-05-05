using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal interface DatabaseStorage {
        /// <summary>
        /// Findet einen Lehrer anhand seiner Id in der Tabelle.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        object FindTeacherById(string tableName, int id);
        object FindTeacherByLastname(string tableName, string lastname);
        string GetInfo();

        /// <summary>
        /// Aktualisiert den Datensatz anhand seiner Id
        /// </summary>
        /// <param name="entity"></param>
        EntityMapperDeprecated Update(EntityMapperDeprecated entity);
        /// <summary>
        /// Erstellt einen neuen Datensatz, und gibt
        /// diesen zurück.
        /// </summary>
        /// <param name="entity"></param>
        void Create(EntityMapperDeprecated entity);
        /// <summary>
        /// Löscht den Datensatz anhand seiner Id
        /// </summary>
        /// <param name="entity"></param>
        void Delete(EntityMapperDeprecated entity);

        /// <summary>
        /// Erstellt einen Datensatz in einer Tabelle
        /// </summary>
        /// <param name="entity"></param>
        int InsertSingleEntity(Entity entity);

        /// <summary>
        /// Aktualisiert einen Datensatz in einer Tabelle
        /// </summary>
        /// <param name="entity"></param>
        void UpdateSingleEntity(Entity entity);
    }
}

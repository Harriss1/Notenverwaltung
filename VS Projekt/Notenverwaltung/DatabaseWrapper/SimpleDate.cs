using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    /// <summary>
    /// Zur Benutzung von einem Datums im Format
    /// yyyy-MM-dd
    /// 
    /// Automatische Konvertiertung zwischen String und
    /// DateTime, auschließlich im Format yyyy-MM-dd
    /// 
    /// Für erweiterte Datumsmanipulation nicht vorgesehen.
    /// </summary>
    internal class SimpleDate {
        public DateTime ToDateTime { get; protected set; }
        public string ToText { get; protected set; }
        private SimpleDate() { }
        /// <summary>
        /// Ein Datum erstellen
        /// </summary>
        /// <param name="date">Format: yyyy-MM-dd</param>
        public SimpleDate(string date) {
            this.ToDateTime = DateTime.Parse(date);
            this.ToText = date;
        }
        /// <summary>
        /// Ein Datum erstellen, Achtung: wird ins Format
        /// yyyy-MM-dd konvertiert
        /// </summary>
        /// <param name="date">Format: yyyy-MM-dd</param>
        public SimpleDate(DateTime date) {
            this.ToDateTime = date;
            this.ToText = date.ToString("yyyy-MM-dd");
        }
    }
}

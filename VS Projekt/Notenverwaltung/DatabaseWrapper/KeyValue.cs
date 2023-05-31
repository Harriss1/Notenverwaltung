using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {

    /// <summary>
    /// Einfache Erstellung eines Key-Value Paares
    /// Der einzig erlaubte Konstruktor nutzt Key als ersten Parameter, Value als zweiten
    /// da KeyValue Paare in diesen Programm einmal erstellte Paare
    /// an keiner Stelle mehr geändert werden soll, es soll
    /// eine Hilfsklasse für das Tabellenmapping bleiben.
    /// </summary>
    internal class KeyValue {
        private string key;
        private object value;
        public KeyValue(string key, object value) {
            this.key = key;
            this.value = value;
        }
        private KeyValue() {
            //Default Konstruktur deaktiviert
        }
        public string GetKey() {
            return key;
        }
        public string GetValueString() {
            if (value.GetType() == typeof(DateTime)) {
                return (new SimpleDate((DateTime)value)).ToText;
            }
            return (string)value;
        }
        public int GetValueInt() {
            return int.Parse(this.GetValueString());
        }
        public object GetValue() {
            return value;
        }
    }
}

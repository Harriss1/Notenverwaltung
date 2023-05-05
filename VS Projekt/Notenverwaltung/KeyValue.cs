using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {

    /// <summary>
    /// Einfache Erstellung eines Key-Value Pairs
    /// Der einzig erlaubte Konstruktor nutzt Key als ersten Parameter, Value als zweiten
    /// da KeyValue Paare in diesen Programm einmal erstellte Paare
    /// an keiner Stelle mehr geändert werden soll, es soll
    /// eine Hilfsklasse für das Tabellenmapping bleiben.
    /// </summary>
    internal class KeyValue {
        private string key;
        private string value;
        public KeyValue(string key, string value) {
            this.key = key;
            this.value = value;
        }
        private KeyValue() {
            //Default Konstruktur deaktiviert
        }
        public string GetKey() {
            return key;
        }
        public string GetValue() {
            return value;
        }
    }
}

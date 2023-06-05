using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    /// <summary>
    /// Dependency Injection - zentrale Zugriffsklasse
    /// </summary>
    internal static class GlobalObjects {
        private class ObjectContainer {
            public InterfaceListing keyInterface;
            public object distinctObject; // einzigartig bzw. unique
            public ObjectContainer(InterfaceListing keyInterface, object obj) {
                this.distinctObject = obj;
                this.keyInterface = keyInterface;
            }
        }

        private static List<ObjectContainer> allObjects = new List<ObjectContainer>();

        /// <summary>
        /// return the object thats associated with the given key
        /// if there is no object with that key, it returns null
        /// </summary>
        public static object Get(InterfaceListing key) {
            foreach (ObjectContainer obj in allObjects) {
                if (obj.keyInterface == key) {
                    return obj.distinctObject;
                }
            }
            return null;
        }
        /// <summary>
        /// Stellt das übergebene Objekt mit dem jeweiligen Key an allen
        /// Stellen des Programms zur Verfügung
        /// 
        /// Der Key ist ein einmalig verwendbarer Enumerationswert, welcher
        /// das Interface beschreibt, für welches ein einmaliges einsetzendes
        /// Objekt im Programm existiert.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key">Zu spezifizierendes Dependency Injection Object</param>
        public static void Add(object obj, InterfaceListing key) {
            if (Get(key) != null) {
                throw new ArgumentException("Objekt welches dieses Interface" +
                    "implementiert, existiert bereits. Dependency kann nicht" +
                    "erstellt werden.");
            }
            allObjects.Add(new ObjectContainer(key, obj));
        }
    }
}

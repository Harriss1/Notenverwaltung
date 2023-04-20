using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal static class GlobalObjects {
        private class ObjectContainer {
            public string key;
            public object distinctObject;
            public ObjectContainer(string key, object obj) {
                this.distinctObject = obj;
                this.key = key;
            }
        }

        private static List<ObjectContainer> allObjects = new List<ObjectContainer>();

        /// <summary>
        /// return the object thats associated with the given key
        /// if there is no object with that key, it returns null
        /// </summary>
        public static object Get(String key) {
            foreach (ObjectContainer obj in allObjects) {
                if (obj.key == key) {
                    return obj.distinctObject;
                }
            }
            return null;
        }
        /// <summary>
        /// Stellt das übergebene Objekt mit dem jeweiligen Key an allen
        /// Stellen des Programms zur Verfügung
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        public static void Add(object obj, string key) {
            if (Get(key) != null) {
                throw new ArgumentException("Key existiert bereits");
            }
            allObjects.Add(new ObjectContainer(key, obj));
        }
    }
}

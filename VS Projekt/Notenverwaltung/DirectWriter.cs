using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class DirectWriter {
        /// <summary>
        /// Ausgabe von Konsolennachrichten, wobei einmalig festgelegt werden kann,
        /// welche Art von Nachrichten angezeigt werden.
        /// Warnung werden nicht angezeigt, falls showlevel=Message
        /// Message = nur Benutzernachrichten anzeigen
        /// </summary>
        public enum ShowLevel {
            DEBUG = 0,
            WARN = 1,
            MESSAGE = 2, // Nur Benutzernachrichten, keine Warnungen anzeigen
            ERROR =3,
            NONE = 4
        }
        private static ShowLevel showLevel = ShowLevel.DEBUG;
        private static bool levelAlreadySet = false;
        public static void SetShowLevel(DirectWriter.ShowLevel showLevel) {
            if (levelAlreadySet) {
                return;
            }
            levelAlreadySet = true;
            DirectWriter.showLevel = showLevel;
        }
        public static void Debug(string message) {
            if(showLevel > ShowLevel.DEBUG) {
                return;
            }
            Console.WriteLine(message);
        }
        /// <summary>
        /// Warnung werden nicht angezeigt, falls showlevel=Message
        /// Message = nur Benutzernachrichten anzeigen
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message) {
            if (showLevel > ShowLevel.WARN) {
                return;
            }
            Console.WriteLine(message);
        }
        /// <summary>
        /// Benutzernachrichten beinhalten keine Warnungen
        /// </summary>
        /// <param name="message"></param>
        public static void Msg(string message) {
            if (showLevel > ShowLevel.MESSAGE) {
                return;
            }
            Console.WriteLine(message);
        }
        public static void Error(string message) {
            if (showLevel > ShowLevel.ERROR) {
                return;
            }
            Console.WriteLine(message);
        }

    }
}

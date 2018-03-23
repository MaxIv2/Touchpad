using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServer {
    static class BlacklistManager {
        public static event EventHandler<object> changeEventHandler;

        public static void AddToBlacklist(string macAddress) {
            string currentList = Properties.Settings.Default.Blacklist;
            if (!currentList.Contains(macAddress)) {
                if (currentList == "")
                    Properties.Settings.Default.Blacklist = macAddress;
                else
                    Properties.Settings.Default.Blacklist += ";" + macAddress;
                Properties.Settings.Default.Save();
                changeEventHandler(null, macAddress);
            }
        }

        public static void Remove(string macAddress) {
            string currentList = Properties.Settings.Default.Blacklist;
            if (currentList.Contains(macAddress)) {
                string[] blacklist = currentList.Split(';');
                string newList = "";
                for (int i = 0; i < blacklist.Length; i++) {
                    if (blacklist[i] != macAddress) {
                        if (newList == "")
                            newList += blacklist[i];
                        else
                            newList += ";" + blacklist[i];
                    }
                }
                Properties.Settings.Default.Blacklist = newList;
                Properties.Settings.Default.Save();
            }
        }

        public static bool IsBlacklisted(string macAddress) {
            string currentList = Properties.Settings.Default.Blacklist;
            return currentList.Contains(macAddress);
        }

        public static string[] GetList() {
            return Properties.Settings.Default.Blacklist.Split(';');
        }
    }
}

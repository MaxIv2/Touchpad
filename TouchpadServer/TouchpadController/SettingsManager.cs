using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadController {
    public class SettingsManager {
        public static void Update(string key, object val) {
            Properties.Settings.Default[key] = val;
            Properties.Settings.Default.Save();
        }

        public static object GetValue(string key) {
            return Properties.Settings.Default[key];
        }
    }
}

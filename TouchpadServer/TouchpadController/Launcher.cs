using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ServiceProcess;

namespace TouchpadController {
    class Launcher {
        public static Process StartProcess(string fileName, string args) {
            try {
                Process process = new Process();
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = args;
                process.Start();
                return process;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static void KillPocess(Process p) {
            p.Kill();
            p.Close();
        }

        public static void StopService(string serviceName) {
            ServiceController serviceController = new ServiceController(serviceName);
            if (serviceController.Status.Equals(ServiceControllerStatus.Running))
                serviceController.Stop();
            else
                throw new Exception("Can't stop service");
        }
    }
}

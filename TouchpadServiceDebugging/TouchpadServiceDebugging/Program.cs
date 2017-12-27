using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServiceDebugging {
    class Program {
        static void Main(string[] args) {
            ServiceTest s = new ServiceTest();
            s.OnStart(null);
            Console.ReadLine();
            s.OnStop();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Json {

    class Program {
        static void Main(string[] args) {
            // if you use https the server port must be 443 and if you use http than the port 80
            // otherwise the code will throw an exception
            DiagnosticsReporter reporter = new DiagnosticsReporter("http://localhost:80");
            Console.WriteLine("First");
            System.Threading.Thread.Sleep(5000);
        }
    }
}


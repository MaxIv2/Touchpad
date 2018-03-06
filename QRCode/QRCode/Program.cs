using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode
{
    class Program
    {
        static void Main(string[] args)
        {
            QRCode qr = new QRCode();
            string str = "";
            List<bool> L = QRCode.Generate("hello world", 10, QRCode.ErrorCorrLev.M);
            for (int i = 0; i < L.Count; i++){
                if (L[i]){
                    str += "1";
                }else{
                    str += "0";
                }
            }
            Console.WriteLine(str);
        }
    }
}

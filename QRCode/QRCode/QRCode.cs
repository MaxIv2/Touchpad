using System.Collections.Generic;
using System.Drawing;

namespace QRCode
{
    class QRCode
    {
        public enum ErrorCorrLev { L, M, Q, H };

        public static List<bool> Generate(string data, int size, ErrorCorrLev ErrCorr)
        {
            QRCodeImpl qrcode = new QRCodeImpl(data, size, ErrCorr);
            return qrcode.Encode();
        }
    }
}

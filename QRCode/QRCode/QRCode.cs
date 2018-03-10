using System.Drawing;

namespace QRCode
{
    class QRCode
    {
        public static Bitmap Generate(string data, int size, QRCodeImpl.ErrorCorrectionLevels ErrCorrVer)
        {
            QRCodeImpl qrcode = new QRCodeImpl(data, size, ErrCorrVer);
            return qrcode.Generate();
        }
    }
}

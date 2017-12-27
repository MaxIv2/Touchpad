using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCoder;

namespace TouchpadController {
    public partial class TouchpadController : Form {
        public TouchpadController(string ip, string port) {
            InitializeComponent();
            string valueInQrCode = ip + ":" + port;
            this.QRCodeContatiner.BackgroundImage = RenderQrCode(valueInQrCode);
            this.QRCodeContatiner.Size = RenderQrCode(valueInQrCode).Size;
            this.QRCodeContatiner.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private static Bitmap RenderQrCode(string data) {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(10);
        }
    }
}

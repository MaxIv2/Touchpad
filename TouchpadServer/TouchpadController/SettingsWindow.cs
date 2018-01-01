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
    public partial class SettingsWindow : Form {
        public SettingsWindow() {
            InitializeComponent();
            object ip = TouchpadService.SettingsManager.GetValue("ip");
            object port = TouchpadService.SettingsManager.GetValue("port");
            string valueInQrCode = ip + ":" + port;
            this.QRCodeContainer.BackgroundImage = RenderQrCode(valueInQrCode);
            this.QRCodeContainer.Size = RenderQrCode(valueInQrCode).Size;
            this.QRCodeContainer.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private static Bitmap RenderQrCode(string data) {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(10);
        }
    }
}

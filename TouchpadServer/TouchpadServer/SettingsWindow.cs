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

namespace TouchpadServer {
    public partial class SettingsWindow : Form {
        public SettingsWindow() {
            InitializeComponent();
            object ip = Properties.Settings.Default.ip;
            object port = Properties.Settings.Default.port;
            string valueInQrCode = ip + ":" + port;
            this.QRCodeContainer.BackgroundImage = RenderQrCode(valueInQrCode);
            this.QRCodeContainer.Size = RenderQrCode(valueInQrCode).Size;
            this.QRCodeContainer.SizeMode = PictureBoxSizeMode.StretchImage;
            this.textBox1.Text = valueInQrCode;
        }

        private static Bitmap RenderQrCode(string data) {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(10);
        }
    }
}

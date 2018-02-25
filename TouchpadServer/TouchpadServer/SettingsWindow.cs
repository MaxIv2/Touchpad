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
        public SettingsWindow(string MACAddress) {
            InitializeComponent();
            this.QRCodeContainer.BackgroundImage = RenderQrCode(MACAddress);
            this.QRCodeContainer.Size = RenderQrCode(MACAddress).Size;
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

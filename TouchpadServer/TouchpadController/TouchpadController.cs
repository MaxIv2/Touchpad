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
        private static Bitmap RenderQrCode(string data) {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(10);
        }

        private static object GetRegistryValue(string valueName) {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Touchpad");
            object value = key.GetValue(valueName, "127.0.0.1");
            key.Close();
            return value;
        }

        public TouchpadController() {
            InitializeComponent();
            string ip = (string) GetRegistryValue("ip");
            int port = (int) GetRegistryValue("port");
            string valueInQrCode = ip + ":" + port;
            this.QRCodeContatiner.BackgroundImage = RenderQrCode(valueInQrCode);
            this.QRCodeContatiner.Size = RenderQrCode(valueInQrCode).Size;
            this.QRCodeContatiner.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}

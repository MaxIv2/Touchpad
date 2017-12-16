using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QRCoder;
using System.Drawing;
namespace QRCodeConnect {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static BitmapSource BitmapSourceFromBitmap(Bitmap bitmap) {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
        public static Bitmap GetQrCodeBitmap(string data) {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(100);
        }
        public MainWindow() {
            InitializeComponent();
            MouseServer ms = new MouseServer();
            string serverEndpoint = String.Format("{0}:{1}", ms.GetIP(), ms.GetPort());
            Rick.Text = serverEndpoint;
            Morty.Source = BitmapSourceFromBitmap(GetQrCodeBitmap(serverEndpoint));

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TouchpadServer {
    class SwitchButton : Button {
        public bool isOn { get; private set; }
        private bool disposed;

        public SwitchButton()
            : base() {
            if(MainContext.status!=null)
            switch (MainContext.status.status) {
                case ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE:
                    this.isOn = false;
                    break;
                default:
                    this.isOn = true;
                    break;
            }
            ApplicationEvents.connectionStatusChangedEventHandler += ChangeAppearance;
        }

        protected override void OnPaint(PaintEventArgs pevent) {
            Rectangle rect = pevent.ClipRectangle;
            Graphics gfx = pevent.Graphics;
            gfx.FillRectangle(new SolidBrush(Parent.BackColor), rect);
            using (var path = new GraphicsPath()) {
                int x = rect.X + 1;
                int y = rect.Y + 1;
                int w = rect.Width - 2;
                int h = rect.Height - 2;
                gfx.FillRectangle(isOn ? Brushes.LightGray : Brushes.DarkGray, rect);
                var innerRect = isOn ? new Rectangle(x + w / 2, y, w / 2, h) : new Rectangle(x, y, w / 2, h);
                gfx.FillRectangle(isOn ? Brushes.Green : Brushes.Black, innerRect);
            }
        }

        private void ChangeAppearance(object sender, ConnectionStatusChangedEventArgs e) {
            switch (e.status) {
                case ConnectionStatusChangedEventArgs.ConnectionStatus.OFFLINE:
                    this.isOn = false;
                    break;
                default:
                    this.isOn = true;
                    break;
            }
            Invalidate();//repaint
        }

        public void Dispose2() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing); 
            if (this.disposed)
                return;
            if (disposing) {
                ApplicationEvents.connectionStatusChangedEventHandler -= this.ChangeAppearance;
            }
            this.disposed = true;
        }
    }
}

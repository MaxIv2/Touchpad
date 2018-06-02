using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TouchpadServer {
    sealed class SwitchButton : Button {
        private bool disposed;

        public SwitchButton()
            : base() {
            SubscribeEvents();
            this.Click += SwitchButton_Click;
        }

        void SwitchButton_Click(object sender, EventArgs e) {
            bool isOnline = MainContext.Status.Connected == MainContext.ServerStatus;
            isOnline = isOnline || MainContext.ServerStatus == MainContext.Status.Online;
            if(isOnline)
                GlobalAppEvents.RaiseOfflineRequestEvent(this, new EventArgs());
            else
                GlobalAppEvents.RaiseOnlineRequestEvent(this, new EventArgs());
        }
        protected override void OnPaint(PaintEventArgs pevent) {
            Rectangle rect = pevent.ClipRectangle;
            Graphics gfx = pevent.Graphics;
            bool isOnline = MainContext.Status.Connected == MainContext.ServerStatus;
            isOnline = isOnline || MainContext.ServerStatus == MainContext.Status.Online;
            gfx.FillRectangle(new SolidBrush(Parent.BackColor), rect);
            using (var path = new GraphicsPath()) {
                int x = rect.X + 1;
                int y = rect.Y + 1;
                int w = rect.Width - 2;
                int h = rect.Height - 2;
                gfx.FillRectangle(isOnline ? Brushes.LightGray : Brushes.DarkGray, rect);
                var innerRect = isOnline ? new Rectangle(x + w / 2, y, w / 2, h) : new Rectangle(x, y, w / 2, h);
                gfx.FillRectangle(isOnline ? Brushes.Green : Brushes.Black, innerRect);
            }
        }
        private void OnOnline(object sender, EventArgs e) {
            Invalidate();//repaint
        }
        private void OnOffline(object sender, EventArgs e) {
            Invalidate();//repaint
        }
        private void SubscribeEvents() {
            GlobalAppEvents.Online += this.OnOnline;
            GlobalAppEvents.Offline += this.OnOffline;
        }
        private void Unsubscribe() {
            GlobalAppEvents.Connected -= this.OnOnline;
            GlobalAppEvents.Disconnected -= this.OnOffline;
        }
    }
}

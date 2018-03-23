using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadServer {
    public partial class BlacklistWindow : Form {
        public BlacklistWindow() {
            InitializeComponent();
            this.blacklistDisplay.Items.AddRange(BlacklistManager.GetList());
            this.removeButton.Click += removeButtonClick;
            BlacklistManager.changeEventHandler += OnBlacklistChange;
        }

        public void OnBlacklistChange(object sender, object newAddress) {
            this.blacklistDisplay.Items.Add(newAddress);
        }

        protected override void OnClosed(EventArgs e) {
            BlacklistManager.changeEventHandler -= OnBlacklistChange;
            base.OnClosed(e);
        }

        private void removeButtonClick(object sender, EventArgs e) {
            object blackListedDevice = (object)blacklistDisplay.SelectedItem;
            if (blackListedDevice != null) {
                BlacklistManager.Remove((string)blackListedDevice);
                blacklistDisplay.Items.Remove(blackListedDevice);
            }
        }

        
    }
}

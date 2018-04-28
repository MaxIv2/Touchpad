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
    public sealed partial class BlacklistWindow : Form {
        private static BlacklistWindow form;

        public static BlacklistWindow Form {
            get {
                if (form == null)
                    form = new BlacklistWindow();
                return form;
            }
        }

        private BlacklistWindow() {
            InitializeComponent();
            this.FillList();
            BlacklistManager.changeEventHandler += OnBlacklistChange;
            this.removeButton.Click += removeButtonClick;
        }

        public void FillList() {
            string[][] items = BlacklistManager.GetAllItems();
            foreach (string[] item in items) {
                this.blacklistView.Items.Add(new ListViewItem(item));
            }
        } 

        public void OnBlacklistChange(object sender, EventArgs e) {
            blacklistView.Items.Clear();
            FillList();
        }

        protected override void OnClosed(EventArgs e) {
            BlacklistManager.changeEventHandler -= OnBlacklistChange;
            form = null;
            base.OnClosed(e);
        }

        private void removeButtonClick(object sender, EventArgs e) {
             ListView.SelectedListViewItemCollection blackListedDevices = blacklistView.SelectedItems;
            foreach (ListViewItem item in blackListedDevices) {
                string address = item.SubItems[1].Text;
                BlacklistManager.Delete(address);
            }
        }

        
    }
}

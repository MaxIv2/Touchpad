using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchpadController {
    public partial class ExceptionWindow : Form {
        public ExceptionWindow(Exception message) {
            InitializeComponent();
            ExceptionText.Text = message.Message;
        }
    }
}

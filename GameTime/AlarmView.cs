using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTime
{
    public partial class AlarmView : Form
    {
        public AlarmView(string msg)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            label1.Text = msg;
        }
    }
}

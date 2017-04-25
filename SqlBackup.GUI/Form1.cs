using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlBackup.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var sqlBackupCreator = new SqlBackupCreator();
            var conStr = tbConStr.Text;
            if (conStr == null)
            {
                return;
            }
            sqlBackupCreator.DoBackup(conStr);
        }
    }
}

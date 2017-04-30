﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SqlBackup.WebForms
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnStart.Click += btnClick_Start; // registers the click event with the method below "btnClick_Start"
        }

        private void btnClick_Start(object sender, EventArgs e)
        {
            var sqlBackupCreator = new SqlBackupCreator();
            var conStr = tbConStr.Text;
            sqlBackupCreator.DoBackup(conStr);
        }
    }
}
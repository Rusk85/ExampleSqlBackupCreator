using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().StartPowershellScript();
        }

        public void StartPowershellScript()
        {
            var mssqlCreator = new MsSqlPowershellRunner();
            var files = Directory.EnumerateFiles(AppContext.BaseDirectory, "*.ps1", SearchOption.AllDirectories);
            var theScript = files.FirstOrDefault();
            mssqlCreator.RunMsSqlExpressServer(File.ReadAllText(theScript), new MsSqlCreationParameters());
        }
    }
}

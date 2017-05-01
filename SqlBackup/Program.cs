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
            MsSqlCreationParameters parameters = new MsSqlCreationParameters
            {
                PathToScript = @"C:\Users\Sven\Documents\local-git-repos\SqlBackup\SqlBackup\run_sqlexpress_docker.ps1",
                HttpLinkToDbMdfFile = "https://goo.gl/8CFlLQ",
                SuAdminPasswort = "XX5S4a5DpvDlWkY*zZl*",
                HostPort = "6999",
                PathToMdfFile = @"C:\Users\Sven\Documents\local-git-repos\SqlBackup\SqlBackup",
                NameOfDatabaseFile = "AdventureWorks2012_Data.mdf",
                DbName = "AdventureWorks2012_Data"
            };
            MsSqlPowershellRunner psRunner = new MsSqlPowershellRunner();
            psRunner.RunMsSqlExpressServer(parameters);
        }
    }
}

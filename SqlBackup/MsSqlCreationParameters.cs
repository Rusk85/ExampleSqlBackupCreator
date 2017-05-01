using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlBackup
{
    class MsSqlCreationParameters
    {
        public string HttpLinkToDbMdfFile { get; set; }
        public string NameOfDatabaseFile { get; set; }
        public string HostPort { get; set; }
        public string SuAdminPasswort { get; set; }
        public string PathToMdfFile { get; set; }
        public string PathToScript { get; set; }
        public string DbName { get; set; }

        public string ScriptContents => File.ReadAllText(PathToScript);

        public string DoubleSlashPathToMdfFile
        {
            get
            {
                string pattern = @"\\";
                Regex regex = new Regex(pattern);
                return regex.Replace(PathToMdfFile, @"\\\\");
            }
        }
    }
}
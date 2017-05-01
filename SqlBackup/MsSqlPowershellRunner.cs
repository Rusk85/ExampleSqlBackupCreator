using System.Linq;
using System.Management.Automation;

namespace SqlBackup
{
    internal class MsSqlPowershellRunner
    {
        public string ContainerGuid { get; private set; }

        public string RunMsSqlExpressServer(MsSqlCreationParameters parameters)
        {
            using (var powerShellScript = PowerShell.Create())
            {
                powerShellScript.Commands.AddScript(parameters.ScriptContents);

                powerShellScript.Commands.AddArgument(parameters.HttpLinkToDbMdfFile);
                powerShellScript.Commands.AddArgument(parameters.NameOfDatabaseFile);
                powerShellScript.Commands.AddArgument(parameters.DoubleSlashPathToMdfFile);
                powerShellScript.Commands.AddArgument(parameters.PathToMdfFile);
                powerShellScript.Commands.AddArgument(parameters.SuAdminPasswort);
                powerShellScript.Commands.AddArgument(parameters.HostPort);
                powerShellScript.Commands.AddArgument(parameters.DbName);

                var output = powerShellScript.Invoke();
                ContainerGuid = output.FirstOrDefault()?.ImmediateBaseObject as string;
            }
            return ContainerGuid;
        }
    }
}
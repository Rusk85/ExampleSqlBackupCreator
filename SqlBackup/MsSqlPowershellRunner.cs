using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace SqlBackup
{
    class MsSqlPowershellRunner
    {

        public string RunMsSqlExpressServer(string scriptText, MsSqlCreationParameters parameters)
        {

            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();
                using (RunspaceInvoke runspaceInvoker = new RunspaceInvoke(runspace))
                {
                    using (PowerShell powerShellScript = PowerShell.Create())
                    {

                        powerShellScript.Runspace = runspace;
                        powerShellScript.Commands.AddScript(scriptText);

                        //[string]$DB_HTTP_LINK,
                        //[string]$DB_FILENAME,
                        //[string]$DB_DIR,
                        //[string]$DB_DIR_NORMAL,
                        //[string]$PASSWORD,
                        //[string]$PORT
                        powerShellScript.Commands.AddArgument(parameters.HttpLinkToDbMdfFile);
                        powerShellScript.Commands.AddArgument(parameters.NameOfDatabaseFile);
                        powerShellScript.Commands.AddArgument(parameters.DoubleSlashPathToMdfFile);
                        powerShellScript.Commands.AddArgument(parameters.PathToMdfFile);
                        powerShellScript.Commands.AddArgument(parameters.SuAdminPasswort);
                        powerShellScript.Commands.AddArgument(parameters.HostPort);

                        powerShellScript.Invoke();
                        var errors = powerShellScript.Streams.Error.ReadAll();
                        foreach (ErrorRecord error in errors)
                        {
                            Debug.WriteLine(error.ErrorDetails.Message);
                        }
                    }
                    
                }
                runspace.Close();
                
            }

            return null;
        }
    }
}
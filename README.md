**tl;dr**

- Example project for exporting a database exists in a github repo: https://github.com/Rusk85/ExampleSqlBackupCreator
- It contains the exporter logic `SqlBackup` as a project
- It contains a `WinForms` project showing the usage of a button click event
- It contains a `WebForms` project showing the same button click event usage
- In addition there is a powershell script allowing to download a `Microsoft Sql Server 2016 Express` instance, automatically loading it up with your desired `database.mdf` file
 and then export it using one of the two GUI projects 

<hr>
I also came across [this SO question](http://stackoverflow.com/a/9835603/1352384) handling a very similar scenario.
There they are linking to this [site](http://www.mssqltips.com/sqlservertip/1849/backup-and-restore-sql-server-databases-programmatically-with-smo/) which is providing multiple methods of backing up a SQL Database from C# using [`Microsoft.SqlServer.Management.Smo`](https://msdn.microsoft.com/en-us/library/microsoft.sqlserver.management.smo.backup(v=sql.120).aspx) Namespace. I have not tried it myself but I think you might find all the information there you need.
I also went ahead and modified the code a bit adding the appropriate using statements. 

I am assuming you have a `ConnectionString` for your SqlServer akin to this one: `Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;` 

More exmaples can be found [here](https://www.connectionstrings.com/sql-server/).
Just pass that connection string to the `DoBackup()` method and make sure you have specified in your connection string the keyword `Database`. The `SqlConnectionStringBuilder` will essentially parse that string and make parts of it accessible through its properties.
<hr>

### Docker Container added for `Microsoft Sql Server Express 2016`
As of right now there is a powershell script `run_sqlexpress_docker.ps1` with some variables that have to be set accordingly but once done automatically pulls the latest MSSQLExpress 2016 Server, downloads any user-specified `database.mdf` file and attaches it to the sql server instance running in the docker container.

**Todos as of know:**

[ ] Work the required parameters for the powershell script into the `WebForms` project

[ ] Run the powershell script from within the project after said variables are set

[ ] Then actually test the 'SqlBackupCreator' after allowing to parameterize some of the static file export fields


<hr>

### Button Click Event (`ASP.NET WebForms`)


_Now as for having this all happening with a click of a button I added the below class `_Default`, which shows just that. The whole project can be found [here](https://github.com/Rusk85/ExampleSqlBackupCreator).
I have to warn you though, I did not have a MSSQLDb to test this and while I was trying to make it work with MySql I just ran out of time. There also is little to no guarding when it comes to malformed `ConnectionStrings`. In other words the app will crash and burn fantastically in cases of that.
There also are static path references for the output file, which you will have to change accordingly._
Interesting parts to watch out for:

- `btnStart.Click += btnClick_Start;` // wiring up the website button to a specific method
- `id="btnStart"` // giving the button a name so I can address it in the CodeBehind
- some more styling candy and stuff; really nothing impressing at all

Already pushed this to my github repo so you can clone it from there and have a got at it: https://github.com/Rusk85/ExampleSqlBackupCreator

<hr>

### WebForm-Project showing `Button Click Event`

This is the code of the `Default.aspx.cs`

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


And this is the code of the `Default.aspx` (at least the juicy bits; the rest is being created when you make a new project anyway)

	<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

		<div class="jumbotron">
			<h2>Clicking the button starts backing up the Database</h2>
			<p>

				<asp:TextBox
					runat="server" ID="tbConStr"
					placeholder="Connection String goes here (no validation)"
					Columns="129"/>
			</p>
			<div style="margin-left: 17.5em">
				<p>
					<asp:Button
						id="btnStart"
						runat="server"
						Text="Start Backup"/>
				</p>
			</div>
		</div>
	</asp:Content>

<hr>

### Button Click Event (`.NET WinForms`)

_Now as for having this all happening with a click of a button I added the below class `Form1`, which shows just that. The whole project can be found [here](https://github.com/Rusk85/ExampleSqlBackupCreator).
I have to warn you though, I did not have a MSSQLDb to test this and while I was trying to make it work with MySql I just ran out of time. There also is little to no guarding when it comes to malformed `ConnectionStrings`. In other words the app will crash and burn fantastically in cases of that.
There also are static path references for the output file, which you will have to change accordingly._
<hr>

You will also need to reference these Assemblies on which you can read up more on [here](https://www.mssqltips.com/sqlservertip/1826/getting-started-with-sql-server-management-objects-smo/):

- Microsoft.SqlServer.ConnectionInfo.dll
- Microsoft.SqlServer.Smo.dll
- Microsoft.SqlServer.SmoEnum.dll
- Microsoft.SqlServer.SqlEnum.dll
- Microsoft.SqlServer.Management.Sdk.Sfc.dll    
    
<hr>
	
	using System;
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
					return;
				sqlBackupCreator.DoBackup(conStr);
			}
		}
	}

<hr>

	using System;
	using System.Data.SqlClient;
	using Microsoft.SqlServer.Management.Common;
	using Microsoft.SqlServer.Management.Smo;

	namespace SqlBackup
	{
		public class SqlBackupCreator
		{
			public static void DoBackup(string connectionString)
			{
				var sqlConStrBldrBuilder = new SqlConnectionStringBuilder(connectionString);
				var bkpDBFull = new Backup();
				/* Specify whether you want to back up database or files or log */
				bkpDBFull.Action = BackupActionType.Database;
				/* Specify the name of the database to back up */
				bkpDBFull.Database = sqlConStrBldrBuilder.InitialCatalog;
				/* You can take backup on several media type (disk or tape), here I am
				 * using File type and storing backup on the file system */
				bkpDBFull.Devices.AddDevice(@"D:\AdventureWorksFull.bak", DeviceType.File);
				bkpDBFull.BackupSetName = "Adventureworks database Backup";
				bkpDBFull.BackupSetDescription = "Adventureworks database - Full Backup";
				/* You can specify the expiration date for your backup data
				 * after that date backup data would not be relevant */
				bkpDBFull.ExpirationDate = DateTime.Today.AddDays(10);

				/* You can specify Initialize = false (default) to create a new 
				 * backup set which will be appended as last backup set on the media. You
				 * can specify Initialize = true to make the backup as first set on the
				 * medium and to overwrite any other existing backup sets if the all the
				 * backup sets have expired and specified backup set name matches with
				 * the name on the medium */
				bkpDBFull.Initialize = false;

				/* Wiring up events for progress monitoring */
				bkpDBFull.PercentComplete += CompletionStatusInPercent;
				bkpDBFull.Complete += Backup_Completed;

				/* SqlBackup method starts to take back up
				 * You can also use SqlBackupAsync method to perform the backup 
				 * operation asynchronously */
				var myServer = new Server(new ServerConnection(new SqlConnection(connectionString)));
				bkpDBFull.SqlBackup(myServer);
			}

			private static void CompletionStatusInPercent(object sender, PercentCompleteEventArgs args)
			{
				Console.Clear();
				Console.WriteLine("Percent completed: {0}%.", args.Percent);
			}

			private static void Backup_Completed(object sender, ServerMessageEventArgs args)
			{
				Console.WriteLine("Hurray...Backup completed.");
				Console.WriteLine(args.Error.Message);
			}

			private static void Restore_Completed(object sender, ServerMessageEventArgs args)
			{
				Console.WriteLine("Hurray...Restore completed.");
				Console.WriteLine(args.Error.Message);
			}
		}
	}
	

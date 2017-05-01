# Downloads or uses existing <database>.mdf file and attaches it to the mssqlexpress instance in the container
# Important Note on Windows Hosts: Since the loopback device doesn't work correctly, you have to connecto to the containers internal IP address
# Find that out by issuing this command: docker inspect <name of container> and look for a key named 'IPAddress'. The IP usually starts with 172.x.x.x

# STARTREGION VARIABLES YOU HAVE TO DEFINE
param
(
	[Parameter(Mandatory=$true,Position=1)]
	[string]$DB_HTTP_LINK,
	[Parameter(Mandatory=$true,Position=2)]
	[string]$DB_FILENAME,
	[Parameter(Mandatory=$true,Position=3)]
	[string]$DB_DIR,
	[Parameter(Mandatory=$true,Position=4)]
	[string]$DB_DIR_NORMAL,
	[Parameter(Mandatory=$true,Position=5)]
	[string]$PASSWORD,
	[Parameter(Mandatory=$true,Position=6)]
	[string]$PORT,
	[Parameter(Mandatory=$true,Position=7)]
	[string]$DB_NAME
)
[System.Console]::WriteLine($DB_HTTP_LINK)
[System.Console]::WriteLine($DB_FILENAME)
[System.Console]::WriteLine($DB_DIR)
[System.Console]::WriteLine($DB_DIR_NORMAL)
[System.Console]::WriteLine($PASSWORD)
[System.Console]::WriteLine($PORT)
#$PORT=1433
#$DB_FILENAME="AdventureWorks2012_Data.mdf" # name your choose for your database
#$DB_DIR="C:\\Users\\Sven\\Documents\\local-git-repos\\SqlBackup\\" # Directory where the database.mdf file will be downloaded
#$DB_DIR_NORMAL="C:\Users\Sven\Documents\local-git-repos\SqlBackup\" # Quite the same as above but with single slashes '\'
#$PASSWORD="XX5S4a5DpvDlWkY*zZl*" # password for your 'sa' user
#$DB_HTTP_LINK="https://goo.gl/8CFlLQ" # link to the database.mdf file
# ENDREGION VARIABLES YOU HAVE TO DEFINE

# DONT CHANGE THIS BLOCK BELOW
$ErrorAction= "Stop";
$ErrorActionPreference = "Stop";
$httpClient=new-object System.Net.WebClient
$DOCKER="docker"
$run="run"
$d="-d"
$p="-p${PORT}:1433"
$e1="-esa_password=$PASSWORD"
$e2="-eACCEPT_EULA=Y"
$volume="-v${DB_DIR_NORMAL}:C:/temp/"
$e3="-eattach_dbs="
$dbToAttach="[{'dbName':'$DB_NAME','dbFiles':['C:\\temp\\${DB_FILENAME}']}]"
$IMAGE="microsoft/mssql-server-windows-express"
# DONT CHANGE THIS BLOCK ABOVE

[System.Console]::WriteLine("Downloading your database. Depending on its size this might take a while.")
$fullDbFilePath=(Join-Path ${DB_DIR} ${DB_FILENAME});
if([System.IO.File]::Exists($fullDbFilePath))
{
	[System.Console]::WriteLine("The database file already exists. Skipping download")
	$dbFileNoExtension=[System.IO.Path]::GetFileNameWithoutExtension((Join-Path ${DB_DIR} ${DB_FILENAME}))
	if([System.IO.File]::Exists((Join-Path $DB_DIR $dbFileNoExtension) + ".ldf"))
	{
		[System.IO.File]::Delete((Join-Path $DB_DIR $dbFileNoExtension) + ".ldf")
	}
}
else
{
	$httpClient.DownloadFile(${DB_HTTP_LINK},(Join-Path ${DB_DIR} ${DB_FILENAME}))
}


[System.Console]::WriteLine("Finished Downloading. Starting SqlExpress Server and attaching your database.")
& $DOCKER $run $d $p $volume ${e3}${dbToAttach} $e2 $e1 $IMAGE
[System.Console]::WriteLine("All finished. Connect using the ip assigned to the container.")

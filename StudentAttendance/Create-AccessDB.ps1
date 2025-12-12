<#
PowerShell script to create `AttendanceDB.accdb` and populate it with tables and sample data.
Run this on Windows with Microsoft Access Database Engine (ACE) installed.
Usage (PowerShell as Admin if needed):
    ./Create-AccessDB.ps1 -OutputPath .\App_Data\AttendanceDB.accdb
#>
param(
    [string]$OutputPath = "$PSScriptRoot\App_Data\AttendanceDB.accdb"
)

# Ensure output directory
$dir = Split-Path $OutputPath -Parent
if (-not (Test-Path $dir)) { New-Item -ItemType Directory -Path $dir | Out-Null }

Write-Host "Creating Access DB at: $OutputPath"

# Create Access DB using ADOX Catalog (requires ACE OLEDB provider installed)
try {
    $catalog = New-Object -ComObject ADOX.Catalog
    $connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=$OutputPath;"
    $catalog.Create($connStr)
    $catalog = $null
    Write-Host "Database file created."
} catch {
    Write-Error "Failed to create .accdb file. Ensure Microsoft Access Database Engine (ACE) is installed and PowerShell process bitness matches the provider. Error: $_"
    exit 1
}

# Define DDL statements (Access SQL)
$ddl = @(
@"
CREATE TABLE Students (
  StudentID COUNTER PRIMARY KEY,
  RegNo TEXT(50),
  FirstName TEXT(255),
  LastName TEXT(255),
  DOB DATETIME,
  [Class] TEXT(255),
  Section TEXT(255),
  Gender TEXT(50),
  PhotoPath TEXT(255)
);
"@,
@"
CREATE TABLE Attendance (
  AttendanceID COUNTER PRIMARY KEY,
  StudentID LONG,
  AttDate DATETIME,
  [Status] TEXT(50),
  Remarks TEXT(255)
);
"@
)

# Execute DDL via OleDb
Add-Type -AssemblyName System.Data
$cn = New-Object System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=$OutputPath;")
try {
    $cn.Open()
    foreach ($s in $ddl) {
        $cmd = $cn.CreateCommand()
        $cmd.CommandText = $s
        $cmd.ExecuteNonQuery() | Out-Null
    }
    Write-Host "Tables created."

    # Insert sample students
    $students = @(
        @{RegNo='REG001';FirstName='John';LastName='Doe';DOB='2008-05-12';Class='7';Section='A';Gender='Male';PhotoPath='';},
        @{RegNo='REG002';FirstName='Jane';LastName='Smith';DOB='2009-08-03';Class='7';Section='A';Gender='Female';PhotoPath='';},
        @{RegNo='REG003';FirstName='Ali';LastName='Khan';DOB='2008-11-20';Class='7';Section='B';Gender='Male';PhotoPath='';},
        @{RegNo='REG004';FirstName='Maria';LastName='Gonzalez';DOB='2009-01-15';Class='6';Section='A';Gender='Female';PhotoPath='';},
        @{RegNo='REG005';FirstName='Chen';LastName='Li';DOB='2008-03-02';Class='7';Section='B';Gender='Male';PhotoPath='';}
    )

    foreach ($st in $students) {
        $cmd = $cn.CreateCommand()
        $cmd.CommandText = "INSERT INTO Students (RegNo,FirstName,LastName,DOB,[Class],Section,Gender,PhotoPath) VALUES (?,?,?,?,?,?,?,?)"
        $cmd.Parameters.Clear()
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p1", $st.RegNo))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p2", $st.FirstName))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p3", $st.LastName))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p4", [datetime]$st.DOB))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p5", $st.Class))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p6", $st.Section))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p7", $st.Gender))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p8", $st.PhotoPath))) | Out-Null
        $cmd.ExecuteNonQuery() | Out-Null
    }
    Write-Host "Sample students inserted."

    # Insert sample attendance for two dates
    $attendance = @(
        @{StudentID=1;AttDate='2025-12-10';Status='Present';Remarks='';},
        @{StudentID=2;AttDate='2025-12-10';Status='Absent';Remarks='Sick';},
        @{StudentID=3;AttDate='2025-12-10';Status='Present';Remarks='';},
        @{StudentID=4;AttDate='2025-12-10';Status='Late';Remarks='Traffic';},
        @{StudentID=5;AttDate='2025-12-10';Status='Present';Remarks='';},
        @{StudentID=1;AttDate='2025-12-11';Status='Absent';Remarks='Sick';},
        @{StudentID=2;AttDate='2025-12-11';Status='Present';Remarks='';},
        @{StudentID=3;AttDate='2025-12-11';Status='Present';Remarks='';}
    )

    foreach ($a in $attendance) {
        $cmd = $cn.CreateCommand()
        $cmd.CommandText = "INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (?,?,?,?)"
        $cmd.Parameters.Clear()
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p1", [int]$a.StudentID))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p2", [datetime]$a.AttDate))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p3", $a.Status))) | Out-Null
        $cmd.Parameters.Add((New-Object System.Data.OleDb.OleDbParameter("@p4", $a.Remarks))) | Out-Null
        $cmd.ExecuteNonQuery() | Out-Null
    }
    Write-Host "Sample attendance inserted."

} catch {
    Write-Error "Error while creating tables or inserting data: $_"
} finally {
    if ($cn.State -eq 'Open') { $cn.Close() }
}

Write-Host "Done. You can now use the file at: $OutputPath"
Write-Host "If you created the DB in the project folder, run the web app in Visual Studio (F5)."

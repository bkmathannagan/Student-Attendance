DbCreator - Console app to create AttendanceDB.accdb

This small VB.NET console app creates `AttendanceDB.accdb` in the project's `App_Data` folder and populates it with sample data.

Requirements
- .NET Framework 4.8 and Visual Studio (to build the project)
- Microsoft Access Database Engine (ACE) redistributable installed (Provider=Microsoft.ACE.OLEDB.12.0)
- PowerShell/Command Prompt with matching bitness to ACE if you installed 32-bit engine

Build & Run
1. Open `DbCreator.sln` or the `DbCreator.vbproj` in Visual Studio and build it.
2. Run the compiled exe, optionally passing an output path:

```powershell
cd d:\Projects\StudentAttendance\DbCreator\bin\Debug
DbCreator.exe "d:\Projects\StudentAttendance\App_Data\AttendanceDB.accdb"
```

Or run from Visual Studio (F5). The program will create the DB and insert sample rows.

If you encounter COM/provider errors, ensure the ACE OLEDB provider is installed and the process bitness (32/64-bit) matches the provider installation.

# Student Attendance Management System

This is a simple ASP.NET WebForms (VB.NET) Student Attendance Management System using MS Access.

Project structure and run instructions are below.

## Setup

- Place `AttendanceDB.accdb` into the `App_Data` folder of the project (create `App_Data` if missing).
- Ensure `Images` folder exists in project root; student photos will be saved there.
- Open the project in Visual Studio targeting .NET Framework 4.8 and run with IIS Express.

## Files
- `StudentBio.aspx` + code-behind: manage students and upload photos
- `Attendance.aspx` + code-behind: daily attendance entry and update
- `Report.aspx` + code-behind: attendance summary and CSV export
- `App_Code/DataAccess.vb`: DB helper using OleDb
- `Web.config`: connection string for Access DB

## Notes
- The Access provider used is `Microsoft.ACE.OLEDB.12.0`. You may need the Access Database Engine redistributable installed (64-bit vs 32-bit must match app pool / IIS Express bitness).

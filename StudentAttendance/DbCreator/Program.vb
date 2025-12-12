Imports System
Imports System.Data.OleDb
Imports System.IO
Imports System.Globalization
Imports Microsoft.VisualBasic

Module Program
    Sub Main(args As String())
        Try
            Dim outPath As String
            If args.Length > 0 Then
                outPath = args(0)
            Else
                ' Default to project App_Data folder relative to executable
                Dim baseDir = AppDomain.CurrentDomain.BaseDirectory
                outPath = Path.GetFullPath(Path.Combine(baseDir, "..", "App_Data", "AttendanceDB.accdb"))
            End If

            Dim dir = Path.GetDirectoryName(outPath)
            If Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If

            Console.WriteLine("Creating Access DB at: " & outPath)

            ' Create the .accdb using ADOX Catalog COM object (requires ACE OLEDB provider)
            Try
                Dim catalog = CreateObject("ADOX.Catalog")
                Dim connStr = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};", outPath)
                catalog.Create(connStr)
                catalog = Nothing
                Console.WriteLine("Database file created.")
            Catch ex As Exception
                Console.WriteLine("Failed to create .accdb file. Ensure Microsoft Access Database Engine (ACE) is installed and process bitness matches the provider.")
                Console.WriteLine("Error: " & ex.Message)
                Return
            End Try

            Using cn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & outPath & ";")
                cn.Open()

                Using cmd As OleDbCommand = cn.CreateCommand()
                    ' Create Students table
                    cmd.CommandText = "CREATE TABLE Students (StudentID COUNTER PRIMARY KEY, RegNo TEXT(50), FirstName TEXT(255), LastName TEXT(255), DOB DATETIME, [Class] TEXT(255), Section TEXT(255), Gender TEXT(50), PhotoPath TEXT(255))"
                    cmd.ExecuteNonQuery()

                    ' Create Attendance table
                    cmd.CommandText = "CREATE TABLE Attendance (AttendanceID COUNTER PRIMARY KEY, StudentID LONG, AttDate DATETIME, [Status] TEXT(50), Remarks TEXT(255))"
                    cmd.ExecuteNonQuery()

                    Console.WriteLine("Tables created.")

                    ' Insert sample students
                    Dim insertStudentSql = "INSERT INTO Students (RegNo,FirstName,LastName,DOB,[Class],Section,Gender,PhotoPath) VALUES (?,?,?,?,?,?,?,?)"
                    Dim students = New List(Of Tuple(Of String, String, String, DateTime, String, String, String, String)) From {
                        Tuple.Create("REG001","John","Doe",DateTime.ParseExact("2008-05-12","yyyy-MM-dd",CultureInfo.InvariantCulture),"7","A","Male",""),
                        Tuple.Create("REG002","Jane","Smith",DateTime.ParseExact("2009-08-03","yyyy-MM-dd",CultureInfo.InvariantCulture),"7","A","Female",""),
                        Tuple.Create("REG003","Ali","Khan",DateTime.ParseExact("2008-11-20","yyyy-MM-dd",CultureInfo.InvariantCulture),"7","B","Male",""),
                        Tuple.Create("REG004","Maria","Gonzalez",DateTime.ParseExact("2009-01-15","yyyy-MM-dd",CultureInfo.InvariantCulture),"6","A","Female",""),
                        Tuple.Create("REG005","Chen","Li",DateTime.ParseExact("2008-03-02","yyyy-MM-dd",CultureInfo.InvariantCulture),"7","B","Male","")
                    }

                    For Each st In students
                        cmd.CommandText = insertStudentSql
                        cmd.Parameters.Clear()
                        cmd.Parameters.Add(New OleDbParameter("@p1", st.Item1))
                        cmd.Parameters.Add(New OleDbParameter("@p2", st.Item2))
                        cmd.Parameters.Add(New OleDbParameter("@p3", st.Item3))
                        cmd.Parameters.Add(New OleDbParameter("@p4", st.Item4))
                        cmd.Parameters.Add(New OleDbParameter("@p5", st.Item5))
                        cmd.Parameters.Add(New OleDbParameter("@p6", st.Item6))
                        cmd.Parameters.Add(New OleDbParameter("@p7", st.Item7))
                        cmd.Parameters.Add(New OleDbParameter("@p8", st.Item8))
                        cmd.ExecuteNonQuery()
                    Next

                    Console.WriteLine("Sample students inserted.")

                    ' Insert sample attendance
                    Dim insertAttSql = "INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (?,?,?,?)"
                    Dim attendance = New List(Of Tuple(Of Integer, DateTime, String, String)) From {
                        Tuple.Create(1, DateTime.ParseExact("2025-12-10","yyyy-MM-dd",CultureInfo.InvariantCulture), "Present", ""),
                        Tuple.Create(2, DateTime.ParseExact("2025-12-10","yyyy-MM-dd",CultureInfo.InvariantCulture), "Absent", "Sick"),
                        Tuple.Create(3, DateTime.ParseExact("2025-12-10","yyyy-MM-dd",CultureInfo.InvariantCulture), "Present", ""),
                        Tuple.Create(4, DateTime.ParseExact("2025-12-10","yyyy-MM-dd",CultureInfo.InvariantCulture), "Late", "Traffic"),
                        Tuple.Create(5, DateTime.ParseExact("2025-12-10","yyyy-MM-dd",CultureInfo.InvariantCulture), "Present", ""),
                        Tuple.Create(1, DateTime.ParseExact("2025-12-11","yyyy-MM-dd",CultureInfo.InvariantCulture), "Absent", "Sick"),
                        Tuple.Create(2, DateTime.ParseExact("2025-12-11","yyyy-MM-dd",CultureInfo.InvariantCulture), "Present", ""),
                        Tuple.Create(3, DateTime.ParseExact("2025-12-11","yyyy-MM-dd",CultureInfo.InvariantCulture), "Present", "")
                    }

                    For Each a In attendance
                        cmd.CommandText = insertAttSql
                        cmd.Parameters.Clear()
                        cmd.Parameters.Add(New OleDbParameter("@p1", a.Item1))
                        cmd.Parameters.Add(New OleDbParameter("@p2", a.Item2))
                        cmd.Parameters.Add(New OleDbParameter("@p3", a.Item3))
                        cmd.Parameters.Add(New OleDbParameter("@p4", a.Item4))
                        cmd.ExecuteNonQuery()
                    Next

                    Console.WriteLine("Sample attendance inserted.")
                End Using

                cn.Close()
            End Using

            Console.WriteLine("Done. DB created at: " & outPath)
            Console.WriteLine("Press Enter to exit.")
            Console.ReadLine()

        Catch ex As Exception
            Console.WriteLine("Unhandled exception: " & ex.ToString())
        End Try
    End Sub
End Module
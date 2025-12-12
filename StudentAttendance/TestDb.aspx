<%@ Page Language="vb" AutoEventWireup="false" CodeFile="TestDb.aspx.vb" Inherits="TestDb" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Test DB Connection</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container mt-4">
        <h3>Database Connectivity Test</h3>
        <p>Runs a simple query to verify connection to `AttendanceDB.accdb`.</p>
        <asp:Label ID="lblStatus" runat="server" CssClass="fst-italic"></asp:Label>
        <div class="mt-3">
            <asp:Button ID="btnTest" runat="server" Text="Test DB" CssClass="btn btn-primary" OnClick="btnTest_Click" />
            <a href="StudentBio.aspx" class="btn btn-link">Students</a>
            <a href="Attendance.aspx" class="btn btn-link">Attendance</a>
            <a href="Report.aspx" class="btn btn-link">Report</a>
        </div>
        <div class="mt-3">
            <asp:Literal ID="litOutput" runat="server"></asp:Literal>
        </div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
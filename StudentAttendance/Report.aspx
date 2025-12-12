<%@ Page Language="vb" AutoEventWireup="false" CodeFile="Report.aspx.vb" Inherits="Report" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Attendance Report</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container mt-4">
        <h2>Attendance Report</h2>
        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger"></asp:Label>
        <div class="row mb-3">
            <div class="col-md-3">
                <label class="form-label">From</label>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="form-label">To</label>
                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3 align-self-end">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                <asp:Button ID="btnExport" runat="server" Text="Export CSV" CssClass="btn btn-outline-secondary ms-2" OnClick="btnExport_Click" />
            </div>
        </div>

        <asp:GridView ID="gvReport" runat="server" CssClass="table table-sm table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="RegNo" HeaderText="Reg No" />
                <asp:BoundField DataField="FullName" HeaderText="Student" />
                <asp:BoundField DataField="TotalDays" HeaderText="Total Days" />
                <asp:BoundField DataField="PresentCount" HeaderText="Present" />
                <asp:BoundField DataField="AbsentCount" HeaderText="Absent" />
                <asp:BoundField DataField="Percent" HeaderText="% Attendance" DataFormatString="{0:N2}" />
            </Columns>
        </asp:GridView>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
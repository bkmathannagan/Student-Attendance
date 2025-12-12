<%@ Page Language="vb" AutoEventWireup="false" CodeFile="Attendance.aspx.vb" Inherits="Attendance" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Attendance</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container mt-4">
        <h2>Daily Attendance</h2>
        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger"></asp:Label>
        <div class="row mb-3">
            <div class="col-md-3">
                <label class="form-label">Date</label>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3 align-self-end">
                <asp:Button ID="btnLoad" runat="server" Text="Load Students" CssClass="btn btn-primary" OnClick="btnLoad_Click" />
            </div>
        </div>

        <asp:GridView ID="gvAttendance" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" DataKeyNames="StudentID">
            <Columns>
                <asp:BoundField DataField="StudentID" HeaderText="ID" Visible="False" />
                <asp:BoundField DataField="RegNo" HeaderText="Reg No" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                            <asp:ListItem>Present</asp:ListItem>
                            <asp:ListItem>Absent</asp:ListItem>
                            <asp:ListItem>Late</asp:ListItem>
                            <asp:ListItem>Leave</asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remarks">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:Button ID="btnSaveAttendance" runat="server" Text="Save Attendance" CssClass="btn btn-success" OnClick="btnSaveAttendance_Click" />
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
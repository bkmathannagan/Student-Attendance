<%@ Page Language="vb" AutoEventWireup="false" CodeFile="StudentBio.aspx.vb" Inherits="StudentBio" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Student Bio</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container mt-4">
        <h2>Student Management</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
        <div class="card mb-3">
            <div class="card-body">
                <asp:HiddenField ID="hfStudentID" runat="server" />
                <div class="row mb-2">
                    <div class="col-md-3">
                        <label class="form-label">RegNo *</label>
                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">First Name *</label>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Last Name</label>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">DOB</label>
                        <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-md-3">
                        <label class="form-label">Class *</label>
                        <asp:TextBox ID="txtClass" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Section</label>
                        <asp:TextBox ID="txtSection" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Gender</label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">--Select--</asp:ListItem>
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Photo</label>
                        <asp:FileUpload ID="fuPhoto" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="d-flex gap-2">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" />
                </div>
            </div>
        </div>

        <asp:GridView ID="gvStudents" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowCommand="gvStudents_RowCommand">
            <Columns>
                <asp:BoundField DataField="StudentID" HeaderText="ID" Visible="False" />
                <asp:BoundField DataField="RegNo" HeaderText="Reg No" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                <asp:BoundField DataField="Class" HeaderText="Class" />
                <asp:BoundField DataField="Section" HeaderText="Section" />
                <asp:TemplateField HeaderText="Photo">
                    <ItemTemplate>
                        <asp:Image runat="server" ID="imgPhoto" ImageUrl='<%# Eval("PhotoPath") %>' Width="60" Height="60" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" CommandName="EditRow" CommandArgument='<%# Eval("StudentID") %>' Text="Edit" CssClass="btn btn-sm btn-outline-primary me-1" />
                        <asp:Button runat="server" CommandName="DeleteRow" CommandArgument='<%# Eval("StudentID") %>' Text="Delete" CssClass="btn btn-sm btn-outline-danger" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
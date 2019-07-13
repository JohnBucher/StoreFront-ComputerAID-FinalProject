<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/Site.Master" CodeBehind="ServiceTest.aspx.cs" Inherits="Project2_v._2._0.ServiceTest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" runat="server">
    <br />
    <br />
    <!-- Test in order to verify that my WebService functions correctly -->
    <h2>SearchProducts Test</h2>
            <!-- TextBox that contains the search string -->
            <asp:TextBox ID="TextBox1" runat="server" Width="180px"></asp:TextBox>
            <!-- Submit button -->
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
            <!-- GridView to display results -->
            <asp:GridView ID="GridView1" runat="server" CellPadding="5">
            </asp:GridView>
    <br />
    <br />
    <!-- Test in order to verify that my WebService functions correctly -->
    <h2>GetProductDetails Test</h2>
        <!-- TextBox that contains the search string -->
        <asp:TextBox ID="TextBox2" runat="server" Width="180px"></asp:TextBox>
        <!-- Submit button -->
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Submit" />
        <!-- GridView to display results -->
        <asp:GridView ID="GridView2" runat="server" CellPadding="5">
        </asp:GridView>
    <br />
    <br />
</asp:Content>


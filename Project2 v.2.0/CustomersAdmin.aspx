<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="CustomersAdmin.aspx.cs" Inherits="Project2_v._2._0.CustomersAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" runat="server">
    
    <br />

    <!-- GridView to display the list of Users in the DB -->
    <asp:GridView ID="CAGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID" DataSourceID="SqlDataSource1" EmptyDataText="There are no data records to display." AllowSorting="True" HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1px" OnSelectedIndexChanged="CAGrid_SelectedIndexChanged" AllowPaging="True" PageSize="50" CellPadding="5" AutoPostBack="True">
    <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="UserID" DataNavigateUrlFormatString="CustomerAdminDetails.aspx?UserID={0}" DataTextField="UserID" HeaderText="UserID" />
        <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
        <asp:BoundField DataField="EmailAddress" HeaderText="EmailAddress" SortExpression="EmailAddress" />
    </Columns>
</asp:GridView>

    <br />

    <!-- DetailsView that allows for the creation of new Users and does not require a page refresh for a User to appear in the table -->
    <asp:DetailsView ID="CADetails" runat="server" DataSourceID="SqlDataSource1" DefaultMode="Insert" Height="50px" Width="125px" HorizontalAlign="Center" AutoGenerateRows="False" DataKeyNames="UserID" AllowPaging="True" CellPadding="5" AutoPostBack="True" OnItemInserted="CADetails_ItemInserted">
        <Fields>
            <asp:BoundField DataField="inputUserName" HeaderText="UserName" SortExpression="UserName" />
            <asp:BoundField DataField="inputEmailAddress" HeaderText="EmailAddress" SortExpression="EmailAddress" />
            <asp:CommandField ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>

    <br />

    <!-- SqlDataSource -->
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyDBConnectionString1 %>" DeleteCommand="DELETE FROM [User] WHERE [UserID] = @UserID" InsertCommand="spAddUser" SelectCommand="spGetUsers" UpdateCommand="UPDATE [User] SET [UserName] = @UserName, [EmailAddress] = @EmailAddress WHERE [UserID] = @UserID" SelectCommandType="StoredProcedure" InsertCommandType="StoredProcedure">
    <DeleteParameters>
        <asp:Parameter Name="UserID" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="inputUserName" Type="String" />
        <asp:Parameter Name="inputEmailAddress" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="UserID" Type="Int32" />
        <asp:Parameter Name="UserName" Type="String" />
        <asp:Parameter Name="EmailAddress" Type="String" />
    </UpdateParameters>
</asp:SqlDataSource>
</asp:Content>

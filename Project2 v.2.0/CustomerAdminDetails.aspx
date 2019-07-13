<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="CustomerAdminDetails.aspx.cs" Inherits="Project2_v._2._0.CustomerAdminDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" runat="server">

    <br />
    <br />
    <br />

    <!-- GridView to display the list of a specific User's details -->
    <asp:GridView ID="CADGrid" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="UserID" DataSourceID="SqlDataSource2" EmptyDataText="There are no data records to display." CellPadding="5" HorizontalAlign="Center" OnRowDeleting="CADGrid_ItemDeleting">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" />
            <asp:BoundField DataField="UserID" HeaderText="UserID" ReadOnly="True" SortExpression="UserID" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
            <asp:BoundField DataField="EmailAddress" HeaderText="EmailAddress" SortExpression="EmailAddress" />
            <asp:CheckBoxField DataField="IsAdmin" HeaderText="IsAdmin" SortExpression="IsAdmin" />
            <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
            <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" />
            <asp:BoundField DataField="DateModified" HeaderText="DateModified" SortExpression="DateModified" />
            <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" />
        </Columns>
    </asp:GridView>

    <br />
    <br />

    <!-- DataView that allows for the editing of some of the data fields -->
    <asp:DetailsView ID="CADDetails" runat="server" AutoGenerateRows="False" DataKeyNames="UserID" DataSourceID="SqlDataSource2" Height="50px" Width="125px" CellPadding="5" HorizontalAlign="Center" OnItemUpdated="CADDetails_ItemUpdated">
        <Fields>
            <asp:BoundField DataField="UserID" HeaderText="UserID" ReadOnly="True" SortExpression="UserID" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
            <asp:BoundField DataField="EmailAddress" HeaderText="EmailAddress" SortExpression="EmailAddress" />
            <asp:CommandField ShowEditButton="True" />
        </Fields>
    </asp:DetailsView>

    <br />
    <br />
    <br />

    <!-- GridView to display th elist of Address that are useable by this User -->
    <asp:GridView ID="AddressGrid" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="AddressID" DataSourceID="SqlDataSource3" EmptyDataText="There are no data records to display." CellPadding="5" HorizontalAlign="Center" AllowPaging="True" PageSize="50">
        <Columns>
            <asp:BoundField DataField="AddressID" HeaderText="AddressID" ReadOnly="True" SortExpression="AddressID" />
            <asp:BoundField DataField="Address1" HeaderText="Address1" SortExpression="Address1" />
            <asp:BoundField DataField="Address2" HeaderText="Address2" SortExpression="Address2" />
            <asp:BoundField DataField="Address3" HeaderText="Address3" SortExpression="Address3" />
            <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
            <asp:BoundField DataField="StateID" HeaderText="State" SortExpression="State" />
            <asp:BoundField DataField="ZipCode" HeaderText="ZipCode" SortExpression="ZipCode" />
        </Columns>
    </asp:GridView>

    <br />
    <br />

    <!-- SqlDataSources -->
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MyDBConnectionString1 %>" SelectCommand="spGetUser" UpdateCommand="spUpdateUser" UpdateCommandType="StoredProcedure" SelectCommandType="StoredProcedure" DeleteCommand="spDeleteUser" DeleteCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="UserID" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="UserID" QueryStringField="UserID" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="UserID" Type="Int32" />
            <asp:Parameter Name="UserName" Type="String" />
            <asp:Parameter Name="EmailAddress" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MyDBConnectionString1 %>" SelectCommand="spGetUserAddresses" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="UserID" QueryStringField="UserID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>


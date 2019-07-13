<%@ Page Title="ProductAdminDetails" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="ProductAdminDetails.aspx.cs" Inherits="Project2_v._2._0.ProductAdminDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" runat="server">

    <br />
    <br />
    <br />

    <!-- GridView to display the list of a specific Product's details -->
    <asp:GridView ID="PADGrid" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ProductID" DataSourceID="SqlDataSource2" EmptyDataText="There are no data records to display." CellPadding="5" HorizontalAlign="Center" OnRowDeleting="PADGrid_ItemDeleting">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" />
            <asp:BoundField DataField="ProductID" HeaderText="ProductID" ReadOnly="True" SortExpression="ProductID" InsertVisible="False" />
            <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:CheckBoxField DataField="IsPublished" HeaderText="IsPublished" SortExpression="IsPublished" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
            <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price"/>
            <asp:BoundField DataField="ImageFile" HeaderText="ImageFile" SortExpression="ImageFile" />
            <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
            <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" />
            <asp:BoundField DataField="DateModified" HeaderText="DateModified" SortExpression="DateModified" />
            <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" />
        </Columns>
    </asp:GridView>

    <br />
    <br />

    <!-- DataView that allows for the editing of some of the data fields -->
    <asp:DetailsView ID="PADDetails" runat="server" AutoGenerateRows="False" DataKeyNames="ProductID" DataSourceID="SqlDataSource2" Height="50px" Width="300px" CellPadding="5" HorizontalAlign="Center" OnItemUpdated="PADDetails_ItemUpdated">
        <Fields>
            <asp:BoundField DataField="ProductID" HeaderText="ProductID" ReadOnly="True" SortExpression="ProductID" InsertVisible="False" />
            <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:CheckBoxField DataField="IsPublished" HeaderText="IsPublished" SortExpression="IsPublished" />
            <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
            <asp:CommandField ShowEditButton="True" />
        </Fields>
    </asp:DetailsView>

    <!-- FileUpload Controller to insert pictures -->
    <div style="text-align: center">
        <asp:FileUpload ID="ProductImageUpload" runat="server" HorizontalAlign="Center"/>
        <br />
        <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click" />
    </div>

    <br />
    <br />

    <!-- SqlDataSources -->
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MyDBConnectionString1 %>" SelectCommand="spGetProduct" UpdateCommand="spUpdateProduct" UpdateCommandType="StoredProcedure" SelectCommandType="StoredProcedure" DeleteCommand="spDeleteProduct" DeleteCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="ProductID" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="ProductID" QueryStringField="ProductID" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="ProductID" Type="Int32" />
            <asp:Parameter Name="ProductName" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="IsPublished" Type="Boolean" />
            <asp:Parameter Name="Price" Type="Decimal" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>


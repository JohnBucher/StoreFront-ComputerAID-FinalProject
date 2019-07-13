<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/Site.master" CodeBehind="Default.aspx.cs" Inherits="Project2_v._2._0.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">

    <br />

    <h1>STOREFRONT</h1>
    <h2>Required Web Pages:</h2>
    <!-- Default.aspx -->
    <p>- <a href="Default.aspx">Default.aspx</a>: This page will be a default page of the application, 
            where the user is provided with information about the application.  It will function as the landing page of the application.</p>

    <br />

    <!-- ProductsAdmin.aspx -->
    <p>- <a href="ProductsAdmin.aspx">ProductsAdmin.aspx</a>: This page will allow administrators to display 
            a listing of all products in the application for management by the administrator.</p>

    <br />

    <!-- ProductAdminDetails.aspx -->
    <p>- <a href="ProductAdminDetails.aspx">ProductAdminDetails.aspx</a>: This page will allow for the administrator 
            to see all information on a product from the Product table.  Specifically allowing them to 
            update this information as necessary.  </p>
    <p> *(Going to this page directly will lead to no records being displayed because no ProductID has been selected)</p>

    <br />

    <!-- CustomersAdmin.aspx -->
    <p>- <a href="CustomersAdmin.aspx">CustomersAdmin.aspx</a>: This page will allow for administrators to see 
            a listing of all users in the application, and the associated details for each user account.  </p>

    <br />

    <!-- CustomerAdminDetails.aspx -->
    <p>- <a href="CustomerAdminDetails.aspx">CustomerAdminDetails.aspx</a>: This page will allow for updating of basic 
            information for a user, this will include displaying the contents of the User and their Addresses.</p>
    <p> *(Going to this page directly will lead to no records being displayed because no UserID has been selected)</p>

    <br />
    <br />

    <!-- Documentation -->
    <h2>Documentation:</h2>
    <p> -- Author: | Bucher, John |</p>
    <p> -- Create date: | 05/25/2017 |</p>
    <p> -- Description:	| Creates the web site and connections for an online store front. |</p>

</asp:Content>

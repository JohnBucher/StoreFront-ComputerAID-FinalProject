﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Store.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MyDataEntities : DbContext
    {
        public MyDataEntities()
            : base("name=MyDataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ShoppingCartProduct> ShoppingCartProducts { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual int spAddProduct(string productName, string description, Nullable<bool> isPublished, Nullable<decimal> price)
        {
            var productNameParameter = productName != null ?
                new ObjectParameter("ProductName", productName) :
                new ObjectParameter("ProductName", typeof(string));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var isPublishedParameter = isPublished.HasValue ?
                new ObjectParameter("IsPublished", isPublished) :
                new ObjectParameter("IsPublished", typeof(bool));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spAddProduct", productNameParameter, descriptionParameter, isPublishedParameter, priceParameter);
        }
    
        public virtual int spAddShoppingCartItem(Nullable<int> inputUserID, Nullable<int> inputProductID)
        {
            var inputUserIDParameter = inputUserID.HasValue ?
                new ObjectParameter("inputUserID", inputUserID) :
                new ObjectParameter("inputUserID", typeof(int));
    
            var inputProductIDParameter = inputProductID.HasValue ?
                new ObjectParameter("inputProductID", inputProductID) :
                new ObjectParameter("inputProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spAddShoppingCartItem", inputUserIDParameter, inputProductIDParameter);
        }
    
        public virtual ObjectResult<spAddUser_Result> spAddUser(string inputUserName, string inputEmailAddress)
        {
            var inputUserNameParameter = inputUserName != null ?
                new ObjectParameter("inputUserName", inputUserName) :
                new ObjectParameter("inputUserName", typeof(string));
    
            var inputEmailAddressParameter = inputEmailAddress != null ?
                new ObjectParameter("inputEmailAddress", inputEmailAddress) :
                new ObjectParameter("inputEmailAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spAddUser_Result>("spAddUser", inputUserNameParameter, inputEmailAddressParameter);
        }
    
        public virtual int spDeleteOrder(Nullable<int> inputID)
        {
            var inputIDParameter = inputID.HasValue ?
                new ObjectParameter("inputID", inputID) :
                new ObjectParameter("inputID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spDeleteOrder", inputIDParameter);
        }
    
        public virtual int spDeleteProduct(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spDeleteProduct", productIDParameter);
        }
    
        public virtual int spDeleteUser(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spDeleteUser", userIDParameter);
        }
    
        public virtual ObjectResult<spGetOrderProducts_Result> spGetOrderProducts(Nullable<int> inputID)
        {
            var inputIDParameter = inputID.HasValue ?
                new ObjectParameter("inputID", inputID) :
                new ObjectParameter("inputID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetOrderProducts_Result>("spGetOrderProducts", inputIDParameter);
        }
    
        public virtual ObjectResult<spGetOrders_Result> spGetOrders()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetOrders_Result>("spGetOrders");
        }
    
        public virtual ObjectResult<spGetProduct_Result> spGetProduct(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetProduct_Result>("spGetProduct", productIDParameter);
        }
    
        public virtual ObjectResult<spGetProducts_Result> spGetProducts(Nullable<bool> publishedOnly)
        {
            var publishedOnlyParameter = publishedOnly.HasValue ?
                new ObjectParameter("PublishedOnly", publishedOnly) :
                new ObjectParameter("PublishedOnly", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetProducts_Result>("spGetProducts", publishedOnlyParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> spGetShoppingCart(Nullable<int> inputID)
        {
            var inputIDParameter = inputID.HasValue ?
                new ObjectParameter("inputID", inputID) :
                new ObjectParameter("inputID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("spGetShoppingCart", inputIDParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> spGetShoppingCartItems(Nullable<int> cartID)
        {
            var cartIDParameter = cartID.HasValue ?
                new ObjectParameter("cartID", cartID) :
                new ObjectParameter("cartID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("spGetShoppingCartItems", cartIDParameter);
        }
    
        public virtual ObjectResult<spGetUser_Result> spGetUser(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetUser_Result>("spGetUser", userIDParameter);
        }
    
        public virtual ObjectResult<spGetUserAddresses_Result> spGetUserAddresses(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetUserAddresses_Result>("spGetUserAddresses", userIDParameter);
        }
    
        public virtual ObjectResult<spGetUsers_Result> spGetUsers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetUsers_Result>("spGetUsers");
        }
    
        public virtual int spUpdateProduct(Nullable<int> productID, string productName, string description, Nullable<bool> isPublished, Nullable<decimal> price)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var productNameParameter = productName != null ?
                new ObjectParameter("ProductName", productName) :
                new ObjectParameter("ProductName", typeof(string));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var isPublishedParameter = isPublished.HasValue ?
                new ObjectParameter("IsPublished", isPublished) :
                new ObjectParameter("IsPublished", typeof(bool));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spUpdateProduct", productIDParameter, productNameParameter, descriptionParameter, isPublishedParameter, priceParameter);
        }
    
        public virtual int spUpdateUser(Nullable<int> userID, string userName, string emailAddress)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var emailAddressParameter = emailAddress != null ?
                new ObjectParameter("EmailAddress", emailAddress) :
                new ObjectParameter("EmailAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spUpdateUser", userIDParameter, userNameParameter, emailAddressParameter);
        }
    }
}

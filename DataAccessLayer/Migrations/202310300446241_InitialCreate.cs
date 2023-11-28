namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Info = c.String(),
                        RegDate = c.DateTime(nullable: false),
                        ActivityCategory_Id = c.Int(),
                        Customer_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityCategories", t => t.ActivityCategory_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.ActivityCategory_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ActivityCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Phone = c.String(),
                        RegDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.String(),
                        RegDate = c.DateTime(nullable: false),
                        IsCheckedOut = c.Boolean(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false),
                        Customer_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                        Stock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Pic = c.String(),
                        Status = c.Boolean(nullable: false),
                        RegDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reminders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ReminderInfo = c.String(),
                        RegDate = c.DateTime(nullable: false),
                        RemindDate = c.DateTime(nullable: false),
                        IsDone = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductInvoices",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Invoice_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Invoice_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Invoice_Id);
            
            CreateTable(
                "dbo.ReminderUsers",
                c => new
                    {
                        Reminder_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reminder_Id, t.User_Id })
                .ForeignKey("dbo.Reminders", t => t.Reminder_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Reminder_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReminderUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ReminderUsers", "Reminder_Id", "dbo.Reminders");
            DropForeignKey("dbo.Invoices", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Activities", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ProductInvoices", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.ProductInvoices", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Activities", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Activities", "ActivityCategory_Id", "dbo.ActivityCategories");
            DropIndex("dbo.ReminderUsers", new[] { "User_Id" });
            DropIndex("dbo.ReminderUsers", new[] { "Reminder_Id" });
            DropIndex("dbo.ProductInvoices", new[] { "Invoice_Id" });
            DropIndex("dbo.ProductInvoices", new[] { "Product_Id" });
            DropIndex("dbo.Invoices", new[] { "User_Id" });
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            DropIndex("dbo.Activities", new[] { "User_Id" });
            DropIndex("dbo.Activities", new[] { "Customer_Id" });
            DropIndex("dbo.Activities", new[] { "ActivityCategory_Id" });
            DropTable("dbo.ReminderUsers");
            DropTable("dbo.ProductInvoices");
            DropTable("dbo.Reminders");
            DropTable("dbo.Users");
            DropTable("dbo.Products");
            DropTable("dbo.Invoices");
            DropTable("dbo.Customers");
            DropTable("dbo.ActivityCategories");
            DropTable("dbo.Activities");
        }
    }
}

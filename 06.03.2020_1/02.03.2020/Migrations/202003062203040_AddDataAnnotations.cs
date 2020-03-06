namespace _02._03._2020.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Barbers", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Barbers", "Surname", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Barbers", "Patronymic", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "Surname", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "Patronymic", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Services", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Positions", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Positions", "Name", c => c.String());
            AlterColumn("dbo.Services", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Patronymic", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Surname", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Barbers", "Patronymic", c => c.String(nullable: false));
            AlterColumn("dbo.Barbers", "Surname", c => c.String(nullable: false));
            AlterColumn("dbo.Barbers", "Name", c => c.String(nullable: false));
        }
    }
}

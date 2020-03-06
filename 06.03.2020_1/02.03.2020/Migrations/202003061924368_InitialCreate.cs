namespace _02._03._2020.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Barbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Patronymic = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Birthdate = c.DateTime(nullable: false),
                        EmploymentDate = c.DateTime(nullable: false),
                        PositionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Positions", t => t.PositionId, cascadeDelete: true)
                .Index(t => t.PositionId);
            
            CreateTable(
                "dbo.Estimations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BarberId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        EstimationType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.EstimationTypes", t => t.EstimationType_Id)
                .ForeignKey("dbo.Barbers", t => t.BarberId, cascadeDelete: true)
                .Index(t => t.BarberId)
                .Index(t => t.CustomerId)
                .Index(t => t.EstimationType_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Patronymic = c.String(nullable: false),
                        Email = c.String(),
                        Phone = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EstimationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BarberId = c.Int(nullable: false),
                        Day = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Barbers", t => t.BarberId, cascadeDelete: true)
                .Index(t => t.BarberId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BarberId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Barbers", t => t.BarberId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.BarberId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BarberId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Datetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Barbers", t => t.BarberId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.BarberId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.BarberServices",
                c => new
                    {
                        Barber_Id = c.Int(nullable: false),
                        Service_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Barber_Id, t.Service_Id })
                .ForeignKey("dbo.Barbers", t => t.Barber_Id, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.Service_Id, cascadeDelete: true)
                .Index(t => t.Barber_Id)
                .Index(t => t.Service_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Records", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Records", "BarberId", "dbo.Barbers");
            DropForeignKey("dbo.Feedbacks", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Feedbacks", "BarberId", "dbo.Barbers");
            DropForeignKey("dbo.BarberServices", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.BarberServices", "Barber_Id", "dbo.Barbers");
            DropForeignKey("dbo.Schedules", "BarberId", "dbo.Barbers");
            DropForeignKey("dbo.Barbers", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Estimations", "BarberId", "dbo.Barbers");
            DropForeignKey("dbo.Estimations", "EstimationType_Id", "dbo.EstimationTypes");
            DropForeignKey("dbo.Estimations", "CustomerId", "dbo.Customers");
            DropIndex("dbo.BarberServices", new[] { "Service_Id" });
            DropIndex("dbo.BarberServices", new[] { "Barber_Id" });
            DropIndex("dbo.Records", new[] { "CustomerId" });
            DropIndex("dbo.Records", new[] { "BarberId" });
            DropIndex("dbo.Feedbacks", new[] { "CustomerId" });
            DropIndex("dbo.Feedbacks", new[] { "BarberId" });
            DropIndex("dbo.Schedules", new[] { "BarberId" });
            DropIndex("dbo.Estimations", new[] { "EstimationType_Id" });
            DropIndex("dbo.Estimations", new[] { "CustomerId" });
            DropIndex("dbo.Estimations", new[] { "BarberId" });
            DropIndex("dbo.Barbers", new[] { "PositionId" });
            DropTable("dbo.BarberServices");
            DropTable("dbo.Records");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Services");
            DropTable("dbo.Schedules");
            DropTable("dbo.Positions");
            DropTable("dbo.EstimationTypes");
            DropTable("dbo.Customers");
            DropTable("dbo.Estimations");
            DropTable("dbo.Barbers");
        }
    }
}

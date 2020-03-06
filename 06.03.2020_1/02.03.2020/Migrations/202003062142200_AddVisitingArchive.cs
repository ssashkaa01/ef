namespace _02._03._2020.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVisitingArchive : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VisitingArchives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BarberId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        EstimationId = c.Int(nullable: false),
                        FeedbackId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        TotalPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Estimations", t => t.EstimationId, cascadeDelete: true)
                .ForeignKey("dbo.Feedbacks", t => t.FeedbackId)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Barbers", t => t.BarberId)
                .Index(t => t.BarberId)
                .Index(t => t.CustomerId)
                .Index(t => t.ServiceId)
                .Index(t => t.EstimationId)
                .Index(t => t.FeedbackId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitingArchives", "BarberId", "dbo.Barbers");
            DropForeignKey("dbo.VisitingArchives", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.VisitingArchives", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.VisitingArchives", "FeedbackId", "dbo.Feedbacks");
            DropForeignKey("dbo.VisitingArchives", "EstimationId", "dbo.Estimations");
            DropIndex("dbo.VisitingArchives", new[] { "FeedbackId" });
            DropIndex("dbo.VisitingArchives", new[] { "EstimationId" });
            DropIndex("dbo.VisitingArchives", new[] { "ServiceId" });
            DropIndex("dbo.VisitingArchives", new[] { "CustomerId" });
            DropIndex("dbo.VisitingArchives", new[] { "BarberId" });
            DropTable("dbo.VisitingArchives");
        }
    }
}

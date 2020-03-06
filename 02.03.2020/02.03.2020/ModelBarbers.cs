namespace _02._03._2020
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class ModelBarbers : DbContext
    {
        // Your context has been configured to use a 'ModelBarbers' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // '_02._03._2020.ModelBarbers' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ModelBarbers' 
        // connection string in the application configuration file.
        public ModelBarbers()
            : base("name=ModelBarbers")
        {
            Database.SetInitializer<ModelBarbers>(new DropCreateDatabaseAlways<ModelBarbers>()); // (default)
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            
            modelBuilder.Entity<Barber>()
                        .HasMany(s => s.Schedules)
                        .WithRequired(i => i.Barber);

            modelBuilder.Entity<Barber>()
                        .HasMany<Service>(s => s.Services)
                        .WithMany(c => c.Barbers);

            modelBuilder.Entity<Barber>()
                        .HasMany<Estimation>(s => s.Estimations)
                        .WithRequired(i => i.Barber);



            modelBuilder.Entity<VisitingArchive>()
             .HasRequired(s => s.Customer)
             .WithMany(c => c.VisitingArchives)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<VisitingArchive>()
                 .HasRequired(s => s.Barber)
                 .WithMany(c => c.VisitingArchives)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<VisitingArchive>()
                .HasRequired(s => s.Barber)
                .WithMany(c => c.VisitingArchives)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VisitingArchive>()
                .HasRequired(s => s.Feedback)
                .WithMany(c => c.VisitingArchives)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Barber>()
                      .HasMany<VisitingArchive>(s => s.VisitingArchives)
                      .WithRequired(i => i.Barber);

          modelBuilder.Entity<Customer>()
                      .HasMany<VisitingArchive>(s => s.VisitingArchives)
                      .WithRequired(i => i.Customer);

  


        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Barber> Barbers { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<EstimationType> EstimationTypes { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<VisitingArchive> VisitingArchives { get; set; }
        public virtual DbSet<Estimation> Estimations { get; set; }
    }

    public class Barber
    {
        public void Service()
        {
            this.Services = new HashSet<Service>();
        }

        public void Schedule()
        {
            this.Schedules = new HashSet<Schedule>();
        }

        public void Estimation()
        {
            this.Estimations = new HashSet<Estimation>();
        }

        public void VisitingArchive()
        {
            this.VisitingArchives = new HashSet<VisitingArchive>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Patronymic { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
        [Required]
        public DateTime EmploymentDate { get; set; }
        [Required]
        public int PositionId { get; set; }

        public virtual Position Position { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Service> Services { get; set; }
        public ICollection<VisitingArchive> VisitingArchives { get; set; }
        public ICollection<Estimation> Estimations { get; set; }
    }

    public class Customer
    {
        public void VisitingArchive()
        {
            this.VisitingArchives = new HashSet<VisitingArchive>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Patronymic { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

        public ICollection<VisitingArchive> VisitingArchives { get; set; }
    }

    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Barber> Barbers { get; set; }
    }

    public class Service
    {
        public void Barber()
        {
            this.Barbers = new HashSet<Barber>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Price { get; set; }
        public int Duration { get; set; }

        public ICollection<Barber> Barbers { get; set; }
    }

    public class EstimationType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class Schedule
    {
        public int Id { get; set; }
        public int BarberId { get; set; }
        public int Day { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        public virtual Barber Barber { get; set; }
    }

    public class Record
    {
        public int Id { get; set; }
        public int BarberId { get; set; }
        public int CustomerId { get; set; }
        [Required]
        public DateTime Datetime { get; set; }
       
        public virtual Barber Barber { get; set; }
        public virtual Customer Customer { get; set; }
    }

    public class Feedback
    {
        public void VisitingArchive()
        {
            this.VisitingArchives = new HashSet<VisitingArchive>();
        }

        public int Id { get; set; }
        public int BarberId { get; set; }
        public int CustomerId { get; set; }
        [Required]
        public string Message { get; set; }

        public virtual Barber Barber { get; set; }
        public virtual Customer Customer { get; set; }

        public ICollection<VisitingArchive> VisitingArchives { get; set; }
    }

    public class VisitingArchive
    {
        public int Id { get; set; }
        public int BarberId { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int EstimationId { get; set; }
        public int FeedbackId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int TotalPrice { get; set; }
     
        public virtual Barber Barber { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Estimation Estimation { get; set; }
        public virtual Feedback Feedback { get; set; }
        public virtual Service Service { get; set; }
    }

    public class Estimation
    {
        public int Id { get; set; }
        public int BarberId { get; set; }
        public int CustomerId { get; set; }
        public int TypeId { get; set; }

        public virtual Barber Barber { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual EstimationType EstimationType { get; set; }
    }


    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}
namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Collections.Generic;

    public class DBSet : DbContext
    {
        public DBSet()
            : base("name=DBSet")
        {
            Database.SetInitializer(new Initializer<DBSet>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shop>().HasRequired(s => s.City).WithMany(c => c.Shops).HasForeignKey(s => s.CityId);
            modelBuilder.Entity<Shop>().HasRequired(s => s.Director).WithOptional(d => d.Shop);
            modelBuilder.Entity<Shop>().HasMany(s => s.Products).WithMany(p => p.Shops);

            modelBuilder.Entity<Worker>().HasRequired(w => w.Shop).WithMany(s => s.Workers).HasForeignKey(w => w.ShopId);
            modelBuilder.Entity<Worker>().HasRequired(w => w.Type).WithMany(t => t.Workers).HasForeignKey(w => w.TypeId);

            modelBuilder.Entity<Product>().HasRequired(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<City>().HasRequired(c => c.Country).WithMany(c => c.Cities).HasForeignKey(c => c.CountryId);

        }

    
         public virtual DbSet<Shop> Shops { get; set; }
         public virtual DbSet<Worker> Workers { get; set; }
         public virtual DbSet<WorkerType> WorkersTypes { get; set; }
         public virtual DbSet<Director> Directors { get; set; }
         public virtual DbSet<City> Cities { get; set; }
         public virtual DbSet<Country> Countries { get; set; }
         public virtual DbSet<Product> Products { get; set; }
         public virtual DbSet<Category> Categories { get; set; }
    }

    public class Shop
    {
        public Shop()
        {
            Products = new HashSet<Product>();
            Workers = new HashSet<Worker>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        public float? ParkingArea { get; set; }

        public int CityId { get; set; }

        //public int DirectorId { get; set; }

        public virtual City City { get; set; }

        public virtual Director Director { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
        public virtual ICollection<Product> Products{ get; set; }
    }

    public class Worker
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Phone { get; set; }

        public int TypeId { get; set; }

        public int ShopId { get; set; }

        public virtual WorkerType Type { get; set; }

        public virtual Shop Shop { get; set; }

    }

    public class WorkerType
    {
        public WorkerType()
        { 
            Workers = new HashSet<Worker>();
        }


        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsStaticSalary { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
    }

    public class Director
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Education { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        public virtual Shop Shop { get; set; }

        [NotMapped]
        public string FullName { get {
                return $"{FirstName} {LastName}";
            }
        }
    }

    public class City
    {
        public City()
        {
            Shops = new HashSet<Shop>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Shop> Shops { get; set; }
    }

    public class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }

    }

    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }     

        public int CategoryId { get; set; }

        public int Price { get; set; }

        public int? Discount { get; set; }

        [Required]
        public bool IsInStock { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Shop> Shops { get; set; }

    }

    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
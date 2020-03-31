namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class Model : DbContext
    {       
        public Model()
            : base("name=Model")
        {
            Database.SetInitializer(new Initializer());
        }

        public virtual DbSet<Street> Streets { get; set; }
    }

    public class Street
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Index { get; set; }
    }

   
    class Initializer : DropCreateDatabaseIfModelChanges<Model>
    {
        protected override void Seed(Model context)
        {
            base.Seed(context);

            context.Streets.AddRange(new List<Street>
            {
                new Street() { Index = 35053, Name = "Гагаріна"},
                new Street() { Index = 35053, Name = "Шевченка"},
                new Street() { Index = 35053, Name = "Гребенюка"},
                new Street() { Index = 35000, Name = "Михайла Грушевського"},
                new Street() { Index = 35000, Name = "Артилерійська вул."},
            });

            context.SaveChanges();
        }
    }
}
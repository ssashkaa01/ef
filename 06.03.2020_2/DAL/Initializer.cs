using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Initializer<T> : DropCreateDatabaseAlways<DBSet>
    {

        protected override void Seed(DBSet ctx)
        {

            var country1 = ctx.Countries.Add(new Country() { Name = "country1" });
            var country2 = ctx.Countries.Add(new Country() { Name = "country2" });
            var country3 = ctx.Countries.Add(new Country() { Name = "country3" });
            var country4 = ctx.Countries.Add(new Country() { Name = "country4" });

            country1.Cities.Add(new City() { Id = 1, Name = "city1" });
            country1.Cities.Add(new City() { Id = 2, Name = "city2" });
            country1.Cities.Add(new City() { Id = 3, Name = "city3" });
            country1.Cities.Add(new City() { Id = 4, Name = "city4" });
            country2.Cities.Add(new City() { Id = 5, Name = "city5" });
            country2.Cities.Add(new City() { Id = 6, Name = "city6" });
            country3.Cities.Add(new City() { Id = 7, Name = "city7" });
            country3.Cities.Add(new City() { Id = 8, Name = "city8" });
            country4.Cities.Add(new City() { Id = 9, Name = "city9" });
            country4.Cities.Add(new City() { Id = 10, Name = "city10" });


            var shop1 = ctx.Shops.Add(new Shop() { CityId = 1, Name = "Shop1", ParkingArea = 200, Address = "Address1" });
            var shop2 = ctx.Shops.Add(new Shop() { CityId = 6, Name = "Shop2", ParkingArea = 90, Address = "Address2" });
            var shop3 = ctx.Shops.Add(new Shop() { CityId = 8, Name = "Shop3", ParkingArea = 140, Address = "Address3" });
            ctx.SaveChanges();
            var director1 = ctx.Directors.Add(new Director() { Shop = shop1, FirstName = "FirstName1", LastName = "LastName1", Email = "email1@email.com", Phone = "0998887766", Education = "Education1" });
            var director2 = ctx.Directors.Add(new Director() { Shop = shop3, FirstName = "FirstName2", LastName = "LastName2", Email = "email2@email.com", Phone = "0998887767", Education = "Education1" });
            //var director3 = ctx.Directors.Add(new Director() { FirstName = "FirstName3", LastName = "LastName3", Email = "email3@email.com", Phone = "0998887768", Education = "Education1" });

            //ctx.Categories.Add(new Category() { Name = "category1" });
            //ctx.Categories.Add(new Category() { Name = "category2" });
            //ctx.Categories.Add(new Category() { Name = "category3" });
            //ctx.Categories.Add(new Category() { Name = "category4" });
            //ctx.Categories.Add(new Category() { Name = "category5" });
            //ctx.Categories.Add(new Category() { Name = "category6" });

            //ctx.WorkersTypes.Add(new WorkerType() { Id = 1, IsStaticSalary = true, Name = "WorkerType1" });
            //ctx.WorkersTypes.Add(new WorkerType() { Id = 2, IsStaticSalary = true, Name = "WorkerType2" });
            //ctx.WorkersTypes.Add(new WorkerType() { Id = 3, IsStaticSalary = true, Name = "WorkerType3" });
            //ctx.WorkersTypes.Add(new WorkerType() { Id = 4, IsStaticSalary = true, Name = "WorkerType4" });
            //ctx.WorkersTypes.Add(new WorkerType() { Id = 5, IsStaticSalary = true, Name = "WorkerType5" });

            //shop1.Workers.Add(new Worker() { Name = "Name1", Surname = "Surname1", Phone = "0991112233", TypeId = 1 });
            //shop1.Workers.Add(new Worker() { Name = "Name2", Surname = "Surname2", Phone = "0991112234", TypeId = 2 });
            ctx.SaveChanges();
        }
    }
 
}

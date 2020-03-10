using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Logics
    {
        DBSet ctx = new DBSet();

        public IEnumerable<ShopDTO> GetShops()
        {
             return ctx.Shops.Select(i => new ShopDTO() {
                 Id = i.Id,
                 Name = i.Name,
                 Address = i.Address,
                 CityId = i.CityId,
                 ParkingArea = i.ParkingArea
             }).ToList();
        }

        public IEnumerable<WorkerDTO> GetWorkers()
        {
            return ctx.Workers.Select(i => new WorkerDTO() {
                Id = i.Id,
                Name = i.Name,
                Surname = i.Surname,
                Phone = i.Phone,
                ShopId = i.ShopId,
                TypeId = i.TypeId
            }).ToList();
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            return ctx.Products.Select(i => new ProductDTO() {
                Id = i.Id,
                Name = i.Name,
                CategoryId = i.CategoryId,
                Discount = i.Discount,
                IsInStock = i.IsInStock,
                Price = i.Price
            }).ToList();
        }

        public IEnumerable<DirectorDTO> GetDirectors()
        {
            return ctx.Directors.Select(i => new DirectorDTO() {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                Phone = i.Phone,
                Education = i.Education
            }).ToList();
        }

        public void InsertDirector(DirectorDTO i)
        {
            ctx.Directors.Add(new Director()
            {
                //Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                Phone = i.Phone,
                Education = i.Education
            });
        }

        public void UpdateDirector(DirectorDTO i)
        {
            Director d = ctx.Directors.First(f => f.Id == f.Id);


            d.FirstName = i.FirstName;
            d.LastName = i.LastName;
            d.Email = i.Email;
            d.Phone = i.Phone;
            d.Education = i.Education;
            
        }

        public bool SaveAll() 
        {
            try
            {
                ctx.SaveChanges();
            } catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                return false;
            }
            return true;
        }
    }
}

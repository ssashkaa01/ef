using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ShopDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public float? ParkingArea { get; set; }

        public int CityId { get; set; }
    }

    public class WorkerDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Phone { get; set; }

        public int TypeId { get; set; }

        public int ShopId { get; set; }
    }
    
    public class DirectorDTO
    {
        public int Id { get; set; }
    
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Education { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }

    public class ProductDTO
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public int Price { get; set; }

        public int? Discount { get; set; }

        public bool IsInStock { get; set; }
        
    }
}

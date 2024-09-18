using System.ComponentModel.DataAnnotations;

namespace Foodies.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string Hotline { get; set; }
        public int MinPrice { get; set; } = 0;// i think it is not important 
        public int MaxPrice { get; set; }
        public string CusineType { get; set; }
        public virtual List<Rating>? Rateofcustomer { get; set; }
        public virtual List<MenuItem> MenuItems { get; set; }
        //200 - 1400
        public virtual List<Order>? Orderes { get; set; }
        public virtual List<Branch>? Branches { get; set; }
        public virtual Admin AdminOfRestaurant { get; set; }
        

        //var result = employees.Average(x => x.Salary);
        //public int avgrave=avg(x=x.rate).whar(x=>x.id=RestaurantId)
        //var tot = Rating.Select(x => x.rate).ToList();
        //T(x=>x.rate).

    }

}

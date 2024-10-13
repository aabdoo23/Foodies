using Foodies.Common;

namespace Foodies.Models
{
    public class Restaurant: BaseEntity
    {
        public string Name { get; set; }
        public string? Photo { get; set; }
        public string Hotline { get; set; }
        public int? MinPrice { get; set; } = 0;
        public int? MaxPrice { get; set; } = 0;
        public string CuisineType { get; set; }
        public virtual List<Rating>? Ratings { get; set; }
        public virtual List<MenuItem>? MenuItems { get; set; }
        //200 - 1400
        public virtual List<Branch>? Branches { get; set; }
        public virtual List<Customer>? FavouriteCustomers { get; set; }
        public virtual Admin? RestaurantAdmin { get; set; }

        // Calculate the average rating
        [NotMapped]
        public double AverageRating
        {
            get
            {
                if (Ratings == null || Ratings.Count == 0)
                    return 0;

                return (double)Ratings.Average(r => r.Rate)!;
            }
        }


        #region New
        /* [Column(TypeName = "time")]
         public TimeSpan OpeningHour { get; set; }//0-23
         [Column(TypeName = "time")]
         public TimeSpan ClosingHour { get; set; }

         public List<string> Areas { get; set; }*/
        #endregion

        //var result = employees.Average(x => x.Salary);
        //public int avgrave=avg(x=x.rate).whar(x=>x.id=RestaurantId)
        //var tot = Rating.Select(x => x.rate).ToList();
        //T(x=>x.rate).

    }

}

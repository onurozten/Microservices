using System.Drawing.Printing;

namespace FreeCourse.Web.Models.Basket
{
    public class BasketItemViewModel
    {

        public int Quatity { get; set; } = 1;

        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public decimal Price { get; set; }

        private decimal? DiscpuntAppliedPrice;

        public void AppliedDiscount(decimal discountPrice)
        {
            DiscpuntAppliedPrice = discountPrice;
        }

        public decimal GetCurrentPrice => DiscpuntAppliedPrice != null ? DiscpuntAppliedPrice.Value : Price;


    }
}
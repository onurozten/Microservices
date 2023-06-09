namespace FreeCourse.Web.Models.Basket
{
    public class BasketViewModel
    {
        public BasketViewModel()
        {
            BasketItems = new List<BasketItemViewModel>();
        }
        public string UserId { get; set; }

        public string DiscountCode { get; set; }

        public int? DiscountRate{ get; set; }

        private List<BasketItemViewModel> _basketItems;

        public List<BasketItemViewModel> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    _basketItems.ForEach(x =>
                    {
                        var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
                        x.AppliedDiscount(Math.Round(x.Price - discountPrice));
                    });
                }
                return _basketItems;
            }
            set
            {
                _basketItems = value;
            }
        }


        public decimal TotalPrice { get => _basketItems.Sum(x => x.GetCurrentPrice); }

        public bool HasDiscount => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;

        public void CancelDiscount()
        {
            DiscountCode = string.Empty;
            DiscountRate = null;
        }
        public void ApplyDiscount(string code, int rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }
    }
}
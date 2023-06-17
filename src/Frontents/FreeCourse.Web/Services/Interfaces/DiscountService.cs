using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Discount;
using System.Text.Json;

namespace FreeCourse.Web.Services.Interfaces
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscount(string discountCode)
        {
            //"[GetByCode]/{code}"
            var response = await _httpClient.GetAsync($"discounts/getbycode/{discountCode}");

            if (!response.IsSuccessStatusCode)
                return null;

            var discount = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();

            return discount.Data;

        }
    }
}

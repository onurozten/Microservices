using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Photostocks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace FreeCourse.Web.Services.Interfaces
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> Deletephoto(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photo={photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Response<PhotoViewModel>> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return null;

            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

            using var ms = new MemoryStream();
                await photo.CopyToAsync(ms);

            var multipartContent = new MultipartFormDataContent();

            multipartContent.Add(new ByteArrayContent(ms.ToArray()), "photo", randomFileName);

            var response = await _httpClient.PostAsync("photos", multipartContent);

            if (!response.IsSuccessStatusCode)
                return null;
            // hata 
            var viewModel = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();

            return viewModel;
        }
    }
}

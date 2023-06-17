using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Photostocks;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IPhotoStockService
    {
        Task<Response<PhotoViewModel>> UploadPhoto(IFormFile photo);
        Task<bool> Deletephoto(string photoUrl);
    }
}

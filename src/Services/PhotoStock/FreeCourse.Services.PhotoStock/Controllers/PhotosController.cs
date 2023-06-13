using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomCountrollerBase
    {

        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {

            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream, cancellationToken);
                }

                var returnPath = "photos/" + photo.FileName;

                var photoDto = new PhotoDto { Url = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }

            return CreateActionResultInstance(Response<NoContent>.Fail("photo is empty", 400));
        }

        [HttpDelete]
        public IActionResult PhotoDelete(string photo)
        {
            var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo);
            
            if (System.IO.File.Exists(photoPath))
                CreateActionResultInstance(Response<NoContent>.Fail("not found", 404));
                
            System.IO.File.Delete(photoPath);

            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}

using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomCountrollerBase
    {
        [HttpPost]
        public IActionResult Receive()
        {
            return CreateActionResultInstance(Response<NoContent>.Success(200));
        }
    }
}

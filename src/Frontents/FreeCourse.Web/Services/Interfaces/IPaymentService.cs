using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Models.FakePayment;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
    }
}

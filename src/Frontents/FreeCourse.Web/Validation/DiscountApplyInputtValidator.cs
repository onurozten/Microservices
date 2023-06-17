using FluentValidation;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Models.Discount;

namespace FreeCourse.Web.Validation
{
    public class DiscountApplyInputtValidator : AbstractValidator<DiscountApplyInput>
    {

        public DiscountApplyInputtValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("İndirim kupon boş olamaz");
        }

    }
}

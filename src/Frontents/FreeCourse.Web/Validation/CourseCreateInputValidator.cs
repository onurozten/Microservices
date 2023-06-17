using FluentValidation;
using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Validation
{
    public class CourseCreateInputValidator:AbstractValidator<CourseCreateInput>
    {

        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kurs ismi boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Kurs açıklaması boş olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Kurs süresi boş olamaz");
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Kurs fiyatı boş olamaz")
                .ScalePrecision(2,6)
                .WithMessage("Hatalı para formatı");

            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kurs kategorisi boş olamaz");

        }

    }
}

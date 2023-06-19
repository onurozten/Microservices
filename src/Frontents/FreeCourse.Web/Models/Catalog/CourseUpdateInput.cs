using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog
{
    public class CourseUpdateInput
    {
        public string Id { get; set; }

        [Display(Name = "Kurs ismi")]
        public string Name { get; set; }

        [Display(Name = "Fiyatı")]
        public decimal Price { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Kapak Resmi")]
        public string Picture { get; set; }

        public DateTime CreatedTime { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs Kategorisi")]
        public string CategoryId { get; set; }

        [Display(Name = "Kapak Resmi")]
        public IFormFile? PhotoFormFile { get; set; }
    }
}

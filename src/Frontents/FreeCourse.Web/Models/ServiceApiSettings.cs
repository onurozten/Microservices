using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models
{
    public class ServiceApiSettings
    {
        public string IdentityBaseUri { get; set; }
        public string GetawayBaseUri { get; set; }
        public string BasPhotoStockUri { get; set; }
        public ServiceApi Catalog { get; set; }
        public ServiceApi PhotoStock { get; set; }
        public ServiceApi Basket { get; set; }
        public ServiceApi Discount { get; set; }
    }

    public class ServiceApi 
    {
        public string Path { get; set; }
    }
}

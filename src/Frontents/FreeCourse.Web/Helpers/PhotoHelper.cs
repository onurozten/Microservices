﻿using FreeCourse.Web.Models;
using Microsoft.Extensions.Options;

namespace FreeCourse.Web.Helpers
{
    public class PhotoHelper
    {
        private readonly ServiceApiSettings _serviceApiSettings;

        public PhotoHelper(IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public string   GetPhotoStockUrl(string photourl)
        {
            return $"{_serviceApiSettings.BasPhotoStockUri}/photos/{photourl}";
        }
    }
}

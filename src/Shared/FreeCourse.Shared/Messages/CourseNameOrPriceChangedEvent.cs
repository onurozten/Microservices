using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Messages
{
    public class CourseNameOrPriceChangedEvent
    {
        public string CourseId { get; set; }

        public string UpdatedName { get; set; }

        public decimal UpdatedPrice { get; set; }

    }

}

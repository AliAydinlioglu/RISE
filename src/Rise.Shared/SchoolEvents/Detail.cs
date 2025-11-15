using Rise.Shared.StudentActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Shared.SchoolEvents
{

    public static partial class SchoolEventRequest
    {
        public class Detail
        {
            public int Id { get; set; }
        }
    }

    public static partial class SchoolEventResponse
    {
        public class Detail
        {
            public SchoolEventDto.Detail SchoolEvent { get; set; }
        }
    }
}

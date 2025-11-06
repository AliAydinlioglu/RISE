using Rise.Shared.StudentActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Shared.SchoolEvents
{
    public static partial class SchoolEventResponse
    {
        public class Index
        {
            public IEnumerable<SchoolEventDto.Index> SchoolEvents { get; set; } = [];
            public int TotalCount { get; set; }
        }

    }
}
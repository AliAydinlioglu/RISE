using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Shared.UserPreferences
{
    public static partial class UserPreferenceResponse
    {
        public class Preferences
        {
            public UserPreferenceDto.Preferences UserPreferences { get; set; } = default!;
        }
        public class Defaults
        {
            public Dictionary<string, object> DefaultPreferences { get; set; } = default!;
        }
    }
}

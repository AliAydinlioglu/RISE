using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rise.Shared.UserPreferences
{
    public static partial class UserPreferenceRequest
    {

        public class Update
        {
            [Required]
            public Dictionary<string, object> Preferences { get; set; } = new();
        }

        public class UpdateSingle
        {
            [Required]
            public string Key { get; set; } = default!;

            [Required]
            public object Value { get; set; } = default!;
        }
    }
}
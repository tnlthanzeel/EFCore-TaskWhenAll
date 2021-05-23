using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Common.HelperMethods
{
    public static class TimeZone
    {
        public static DateTimeOffset SetClientTimeZone(this DateTimeOffset dateTimeOffset)
        {
            TimeZoneInfo slZone = TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time");

            var clientdatetime = dateTimeOffset.ToOffset(slZone.BaseUtcOffset);
            return clientdatetime;
        }
    }
}

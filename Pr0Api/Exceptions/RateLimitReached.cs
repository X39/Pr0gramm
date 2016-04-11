using System;

namespace Pr0gramm.API.Exceptions
{
    public class RateLimitReached : Exception
    {
        public RateLimitReached() : base("403, Rate Limit Reached")
        {

        }
    }
}

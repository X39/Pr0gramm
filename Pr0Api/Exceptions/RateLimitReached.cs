using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr0gramm.API.Exceptions
{
    public class RateLimitReached : Exception
    {
        public RateLimitReached() : base("403, Rate Limit Reached")
        {

        }
    }
}

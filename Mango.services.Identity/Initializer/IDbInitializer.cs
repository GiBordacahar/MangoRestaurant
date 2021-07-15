using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.services.Identity.Initializer
{
    public interface IDbInitializer
    {
        public void Initialize();
    }
}

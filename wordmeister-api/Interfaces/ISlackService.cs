using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Interfaces
{
    public interface ISlackService
    {
        void PostMessage(object message);
    }
}

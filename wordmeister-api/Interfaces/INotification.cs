using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Interfaces
{
    public interface INotification
    {
        void Message(string body, string subject, string to);
    }
}

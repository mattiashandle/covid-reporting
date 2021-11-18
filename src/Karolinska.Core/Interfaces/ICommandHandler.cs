using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Core.Interfaces
{
    public interface ICommandHandler<U,T>
    {
        Task<T> HandleAsync(U command);
    }
}

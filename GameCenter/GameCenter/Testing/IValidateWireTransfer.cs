using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCenter.Testing
{
    public interface IValidateWireTransfer
    {
        OperationResult Validate(Account origin, Account destination, decimal amount);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCenter.Testing
{
    public class WiretransferValidator : IValidateWireTransfer
    {
        public OperationResult Validate(Account origin, Account destination, decimal amount)
        {
            if (amount > origin.Funds)
            {
                return new OperationResult(false, "The origin account does not have enough funds available");
            }

            // Other validations
            return new OperationResult(true);
        }
    }
}

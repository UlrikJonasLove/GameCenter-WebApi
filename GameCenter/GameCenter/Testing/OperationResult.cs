using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCenter.Testing
{
    public class OperationResult
    {
        public OperationResult(bool isSuccessful, string errorMessage = null)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}

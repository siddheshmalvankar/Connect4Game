using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ka4.Core.DTO.GatewayResponses
{
    public class BaseGatewayResponse
    {
        public bool Success { get; }
        public IEnumerable<Error> Errors { get; }

        protected BaseGatewayResponse(bool success = false, IEnumerable<Error> errors = null)
        {
            Success = success;
            Errors = errors;
        }
    }
}

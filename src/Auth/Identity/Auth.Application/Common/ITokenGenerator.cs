using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Common
{
    public interface ITokenGenerator
    {
        public string GenerateJWTToken((string userId, string userName, IList<string> roles) userDetails);
    }
}

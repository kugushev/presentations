using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomainLayer
{
    public interface ISecurityService
    {
        string SecurityToken { get; }
        int GetCurrentHash(string securityToken);
        int GetCurrentHash(string securityToken, Guid requestId);
    }
}

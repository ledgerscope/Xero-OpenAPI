using System;

namespace Xero.NetStandard.OAuth2.Model
{
    public interface IXeroEntity
    {

    }

    public partial class Account : IXeroEntity {}

    public partial class Attachment : IXeroEntity {}
    
    public partial class Contact : IXeroEntity {}
}
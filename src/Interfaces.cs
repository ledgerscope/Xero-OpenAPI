using System;
using System.Collections.Generic;

namespace Xero.NetStandard.OAuth2.Shared
{
    public interface IHasId
    {
        public Guid Id { get; set; }
    }

    public interface IHasUpdatedDate
    {
        public DateTime UpdatedDateUTC { get; }
    }
    
    public interface IXeroCollection<T, V>
    {
        public V Collection { get; set; }
    }
}

namespace Xero.NetStandard.OAuth2.Model.Accounting
{
    public interface IHasLineItems
    {
        ICollection<LineItem> LineItems { get; }
    }
}
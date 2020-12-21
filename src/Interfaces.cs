using System;
using System.Collections.Generic;

namespace Xero.NetStandard.OAuth2.Model
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

    public interface IHasLineItems
    {
        ICollection<LineItem> LineItems { get; }
    }
}
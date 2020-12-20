using System;
using System.Collections.Generic;

namespace Xero.NetStandard.OAuth2.Model
{
    public interface IHasId
    {
        public Guid Id { get; }
    }

    public interface IHasUpdatedDate
    {
        public DateTime UpdatedDateUTC { get; }
    }
    
    public interface IXeroCollection<T>
    {
        public T Collection { get; }
    }

    public interface IHasLineItems
    {
        ICollection<LineItem> LineItems { get; }
    }
}
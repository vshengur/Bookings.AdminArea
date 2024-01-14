using Bookings.Domain;
using Bookings.Infrastructure.Documents;

namespace Bookings.Infrastructure.Mappers;

public interface IDocumentMapper<TDomain, TDocument>
    where TDomain : BaseObject
    where TDocument : BaseDocument
{
    TDomain FromDocument(TDocument booking);
    TDocument ToDocument(TDomain booking);
}
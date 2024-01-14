﻿using Bookings.Domain;
using Bookings.Infrastructure.Documents;
using Bookings.Infrastructure.Mappers;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bookings.Repositories.Domain;

public class RoomsRepository(IMongoDBContext context, IDocumentMapper<Room, RoomDocument> mapper, ILogger<RoomsRepository> logger)
    : BaseRepository<Room, RoomDocument>(context, mapper, logger), IRoomsRepository
{
}

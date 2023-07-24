﻿using Bookings.Domain;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Repositories.Domain
{
    public class HotelsRepository : BaseRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(IMongoDBContext context)
            : base(context)
        {
        }
    }
}
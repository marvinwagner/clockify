﻿using Clockify.Core.Domain;
using System;

namespace Clockify.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get;  }
    }
}

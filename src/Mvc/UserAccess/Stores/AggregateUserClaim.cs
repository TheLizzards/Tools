﻿using System;
using Microsoft.AspNetCore.Identity;
using Picums.Data.Domain;

namespace Picums.Mvc.UserAccess.Stores
{
    public sealed class AggregateUserClaim : IdentityUserClaim<Guid>, IAggregateRoot
    {
        public Guid Id => this.UserId;
    }
}
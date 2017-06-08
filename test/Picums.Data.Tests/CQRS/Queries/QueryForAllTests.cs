﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Picums.Data.CQRS.DataAccess;
using Picums.Data.CQRS.Queries;
using Picums.Data.InMemory;
using Picums.Data.Tests.Mocks;
using Picums.Tests;
using Xunit;

namespace Picums.Data.Tests.CQRS.Queries
{
    public sealed class QueryForAllTests
    {
        private readonly DatabaseParts parts;
        private readonly IDataContext context;
        private readonly Guid id;

        public QueryForAllTests()
        {
            this.parts = new DatabaseParts("test", "test");
            this.id = Guid.NewGuid();
            var storage = new Dictionary<string, object>
            {
                ["SimpleAggregateRoot"] = new List<SimpleAggregateRoot>
                {
                    new SimpleAggregateRoot(id)
                }
            };
            this.context = new InMemoryDataContext(storage);
        }

        [Fact]
        public async Task QueringForExistingEntity()
        {
            var query = new QueryForAll<SimpleAggregateRoot>(
                    this.context
                    , new TestLoggerFactory()
                    , this.parts);

            var result = await query.Execute();

            result.Count().Should().Be(1);
        }
    }
}
﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.Documents.Client;
using Picums.Data.Azure.Tests.Mocks;
using Picums.Data.CQRS.DataAccess;
using Picums.Tests;

namespace Picums.Data.Azure.Tests.Entities
{
    public sealed class DatabaseAccess : IDisposable
    {
        private readonly AzureDocumentDbContext context;
        private readonly DatabaseParts parts;

        public DatabaseAccess()
        {
            this.context = new AzureDocumentDbContext(new InjectableAzureDocumentOptions(), new TestLoggerFactory());
            this.parts = new DatabaseParts("Test", "TestItem");
        }

        public async Task SaveDataWithDataWriter()
        {
            await InitialiseDatabase();
            var writer = this.context.GetWriter<TestItem>(this.parts);
            var item = new TestItem();

            await writer.InsertNew(item);

            var returnItem = LoadItem(item.Id);
            returnItem.Should().NotBeNull();
        }

        public void Dispose()
        {
            DropTestDatabase().GetAwaiter().GetResult();
        }

        private async Task InitialiseDatabase()
                    => await new AzureDocumentDbContextInitialiser(
                    new InjectableAzureDocumentOptions()
                    , new TestLoggerFactory())
                .Initialise();

        private async Task DropTestDatabase()
        {
            (var authkey, var endpoint) = new InjectableAzureDocumentOptions();
            var client = new DocumentClient(new Uri(endpoint), authkey);

            var databaseLink = UriFactory.CreateDatabaseUri("Test");
            await client.DeleteDatabaseAsync(databaseLink);
        }

        private async Task<TestItem> LoadItem(Guid id)
            => (await this.context
                .GetReader<TestItem>(this.parts)
                .ById(id))
                .Value;
    };
}
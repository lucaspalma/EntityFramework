// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Specification.Tests;
using Microsoft.EntityFrameworkCore.Specification.Tests.TestModels.Inheritance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Sqlite.FunctionalTests
{
    public class InheritanceSqliteFixture : InheritanceRelationalFixture
    {
        private readonly DbContextOptions _options;

        public TestSqlLoggerFactory TestSqlLoggerFactory { get; } = new TestSqlLoggerFactory();

        public InheritanceSqliteFixture()
        {
            _options = new DbContextOptionsBuilder()
                .UseSqlite(SqliteTestStore.CreateConnectionString("InheritanceSqlite"))
                .UseInternalServiceProvider(new ServiceCollection()
                    .AddEntityFrameworkSqlite()
                    .AddSingleton(TestModelSource.GetFactory(OnModelCreating))
                    .AddSingleton<ILoggerFactory>(TestSqlLoggerFactory)
                    .BuildServiceProvider())
                .Options;

            using (var context = CreateContext())
            {
                context.Database.EnsureClean();
                SeedData(context);
            }
        }

        public override InheritanceContext CreateContext() => new InheritanceContext(_options);
    }
}

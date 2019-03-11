// <copyright file="BaseRepositoryTests.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BackEnd.Infrastructure.Data.Repositories;
    using FluentAssertions;
    using TestHelpers;
    using Xunit;

    [Trait("Class", "BaseRepository")]
    public class BaseRepositoryTests
    {
        private MockCypherClientBuilder<TestEntity> _queryBuilder;

        public BaseRepositoryTests()
        {
            _queryBuilder = new MockCypherClientBuilder<TestEntity>();
        }

        public class GetAllAsync : BaseRepositoryTests
        {
            [Fact]
            public async Task ReturnsAllMatchingNodes()
            {
                // Arrange
                var entities = new List<TestEntity>
                {
                    new TestEntity { Name = "Entity 1" },
                    new TestEntity { Name = "Entity 2" }
                };
                var client = _queryBuilder
                    .WithMatch("(e:TestEntity)")
                    .WithReturnEnumerable(e => e.As<TestEntity>(), entities)
                    .Finalize();

                // Act
                var repository = new BaseRepository<TestEntity>(client.Object);
                var result = await repository.GetAllAsync();

                // Assert
                result.Count.Should().Be(2);
            }
        }

        public class GetByIdAsync : BaseRepositoryTests
        {
            [Fact]
            public void ReturnsSingleMatchingNode()
            {
            }
        }
    }
}

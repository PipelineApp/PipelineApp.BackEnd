namespace PipelineApp.BackEnd.Test.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BackEnd.Infrastructure.Data.Entities;
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
                    new TestEntity {Name = "Entity 1"},
                    new TestEntity {Name = "Entity 2"}
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

// <copyright file="BaseRepositoryTests.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BackEnd.Infrastructure.Data.Entities;
    using BackEnd.Infrastructure.Data.Relationships;
    using BackEnd.Infrastructure.Data.Repositories;
    using FluentAssertions;
    using Moq;
    using Neo4jClient.Cypher;
    using TestHelpers;
    using Xunit;

    [Trait("Class", "BaseRepository")]
    public class BaseRepositoryTests : RepositoryTests<BaseRepository<MockDataEntity>, MockDataEntity>
    {
        public BaseRepositoryTests()
        {
            Repository = new BaseRepository<MockDataEntity>(MockGraphClient.Object);
            SetData(new List<MockDataEntity>()
            {
                new MockDataEntity { Name = "Entity 1" },
                new MockDataEntity { Name = "Entity 2" },
                new MockDataEntity { Name = "Entity 3" }
            });
        }

        public class GetAllAsync : BaseRepositoryTests
        {
            [Fact]
            public async Task FetchesAllNodesOfType()
            {
                // Arrange
                MockGraphClient
                    .Setup(gc => gc.ExecuteGetCypherResultsAsync<MockDataEntity>(It.IsAny<CypherQuery>()))
                    .Returns(Task.FromResult<IEnumerable<MockDataEntity>>(Data));

                // Act
                var results = await Repository.GetAllAsync();

                // Assert
                VerifyQueryWithResults("MATCH(e:MockDataEntity)\r\nRETURN e");
                results.Should().HaveCount(3);
            }
        }

        public class GetByIdAsync : BaseRepositoryTests
        {
            [Fact]
            public async Task FetchesSingleNodeById()
            {
                // Arrange
                MockGraphClient
                    .Setup(gc => gc.ExecuteGetCypherResultsAsync<MockDataEntity>(It.IsAny<CypherQuery>()))
                    .Returns(Task.FromResult<IEnumerable<MockDataEntity>>(Data.GetRange(1, 1)));
                var id = Data[1].Id;

                // Act
                var results = await Repository.GetByIdAsync(id);

                // Assert
                VerifyQueryWithResults($"MATCH(e:MockDataEntity)\r\nWHERE (e.Id = \"{id}\")\r\nRETURN e");
                results.Name.Should().Be("Entity 2");
            }
        }

        public class AddOutboundRelationshipAsync : BaseRepositoryTests
        {
            [Fact]
            public async Task AddsRelationshipToExistingNodes()
            {
                // Arrange
                var sourceId = Guid.NewGuid();
                var targetId = Guid.NewGuid();

                // Act
                await Repository.AddOutboundRelationshipAsync<BelongsToRole, UserEntity>(sourceId, targetId);

                // Assert
                VerifyQuery($"MATCH(source:MockDataEntity),(target:UserEntity)\r\nWHERE(source.Id=\"{sourceId}\")\r\nAND(target.Id=\"{targetId}\")\r\nCREATE(source)-[:BelongsToRole]->(target)");
            }
        }

        public class RemoveOutboundRelationshipAsync : BaseRepositoryTests
        {
            [Fact]
            public async Task RemovesRelationshipFromExistingNodes()
            {
                // Arrange
                var sourceId = Guid.NewGuid();
                var targetId = Guid.NewGuid();

                // Act
                await Repository.RemoveOutboundRelationshipAsync<BelongsToRole, UserEntity>(sourceId, targetId);

                // Assert
                VerifyQuery($"MATCH(source:MockDataEntity)-[r:BelongsToRole]->(target:UserEntity)\r\nWHERE(source.Id=\"{sourceId}\")\r\nAND(target.Id=\"{targetId}\")\r\nDELETEr");
            }
        }

        public class UpdateAsync : BaseRepositoryTests
        {
            [Fact]
            public async Task SavesUpdatedNode()
            {
                // Arrange
                var model = new MockDataEntity { Name = "Entity 4" };
                var startId = model.Id;
                MockGraphClient
                    .Setup(gc => gc.ExecuteGetCypherResultsAsync<MockDataEntity>(It.IsAny<CypherQuery>()))
                    .Returns(Task.FromResult<IEnumerable<MockDataEntity>>(new List<MockDataEntity> { model }));

                // Act
                var results = await Repository.UpdateAsync(model);

                // Assert
                VerifyQueryWithResults($"MATCH(e:MockDataEntity)\r\nWHERE(e.Id=\"{startId}\")\r\nSET e= {{model}}\r\nRETURN e", new Dictionary<string, object> { { "model", model } });
                results.Name.Should().Be("Entity 4");
                results.Id.Should().Be(startId);
            }
        }

        public class DeleteAsync : BaseRepositoryTests
        {
            [Fact]
            public async Task RemovesMatchingNode()
            {
                // Arrange
                var id = Guid.NewGuid();

                // Act
                await Repository.DeleteAsync(id);

                // Assert
                VerifyQuery($"MATCH(e: MockDataEntity)\r\nWHERE(e.Id =\"{id}\")\r\nDETACH DELETE e");
            }
        }

        public class Count : BaseRepositoryTests
        {
            [Fact]
            public async Task ReturnsCountOfMatchingNodes()
            {
                // Arrange
                MockGraphClient
                    .Setup(gc => gc.ExecuteGetCypherResultsAsync<long>(It.IsAny<CypherQuery>()))
                    .Returns(Task.FromResult<IEnumerable<long>>(new List<long> { 54 }));

                // Act
                var results = await Repository.Count();

                // Assert
                VerifyQueryWithResults<long>($"MATCH(e:MockDataEntity)\r\nRETURN count(e)");
                results.Should().Be(54);
            }
        }
    }
}

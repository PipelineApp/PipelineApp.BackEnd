// <copyright file="BaseRepositoryTests.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BackEnd.Infrastructure.Data.Entities;
    using BackEnd.Infrastructure.Data.Repositories;
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
        }

        public class GetAllAsync : BaseRepositoryTests
        {
            [Fact]
            public async Task FetchesAllNodesOfType()
            {
                MockGraphClient
                    .Setup(gc => gc.ExecuteGetCypherResultsAsync<MockDataEntity>(It.IsAny<CypherQuery>()))
                    .Returns(Task.FromResult<IEnumerable<MockDataEntity>>(new List<MockDataEntity>()));

                var results = await Repository.GetAllAsync();

                VerifyQuery("MATCH(e:MockDataEntity)\r\nRETURN e");
            }

            [Fact]
            public async Task CheckingCypher()
            {
                var mockGc = MockGraphClient;
                mockGc
                    .Setup(gc => gc.ExecuteGetCypherResultsAsync<PersonaEntity>(It.IsAny<CypherQuery>()))
                    .Returns(Task.FromResult<IEnumerable<PersonaEntity>>(new List<PersonaEntity>()));

                var expectedQuery = new CypherQuery(
                    "MATCH(persona:PersonaEntity)\r\nWHERE (persona.Slug = \"testSlug\")\r\nRETURN persona",
                    new Dictionary<string, object> { { "p0", "testSlug" } },
                    CypherResultMode.Projection);

                var pr = new BaseRepository<MockDataEntity>(mockGc.Object);
                var results = await pr.GetAllAsync();

                mockGc.Verify(gc => gc.ExecuteGetCypherResultsAsync<PersonaEntity>(It.Is<CypherQuery>((actual) => CompareQuery(expectedQuery, actual, true))), Times.Once);
            }
        }
    }
}

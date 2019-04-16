// <copyright file="RepositoryTests.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using BackEnd.Infrastructure.Data.Repositories;
    using Interfaces.Data;
    using Interfaces.Repositories;
    using Moq;
    using Neo4jClient;
    using Neo4jClient.Cypher;

    public abstract class RepositoryTests<T, T1>
        where T : BaseRepository<T1>
        where T1 : IEntity
    {
        protected T Repository { get; set; }

        protected Mock<IRawGraphClient> MockGraphClient { get; set; }

        protected RepositoryTests()
        {
            var mgc = new Mock<IRawGraphClient>();
            mgc.Setup(gc => gc.Cypher).Returns(new CypherFluentQuery(mgc.Object));
            MockGraphClient = mgc;
        }

        protected void VerifyQuery(string cypherQuery, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            var expectedQuery = new CypherQuery(cypherQuery, parameters, CypherResultMode.Projection);
            MockGraphClient.Verify(gc => gc.ExecuteGetCypherResultsAsync<MockDataEntity>(It.Is<CypherQuery>((actual) => CompareQuery(expectedQuery, actual, true))), Times.Once);
        }

        protected bool CompareQuery(CypherQuery expected, CypherQuery actual, bool ignoreWhitespace = true)
        {
            if (expected == null || actual == null)
            {
                return actual == null && expected == null;
            }

            if (!ignoreWhitespace)
            {
                return expected.DebugQueryText == actual.DebugQueryText;
            }

            return expected.DebugQueryText.Replace(" ", string.Empty, StringComparison.InvariantCulture) == actual.DebugQueryText.Replace(" ", string.Empty, StringComparison.InvariantCulture);
        }
    }
}

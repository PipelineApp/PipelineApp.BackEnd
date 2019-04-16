// <copyright file="RepositoryTests.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BackEnd.Infrastructure.Data.Repositories;
    using Interfaces.Data;
    using Interfaces.Repositories;
    using Moq;
    using Neo4jClient;
    using Neo4jClient.Cypher;

    public abstract class RepositoryTests<TRepository, TEntity> : IDisposable
        where TRepository : BaseRepository<TEntity>
        where TEntity : IEntity
    {
        protected TRepository Repository { get; set; }

        protected List<TEntity> Data { get; }

        protected Mock<IRawGraphClient> MockGraphClient { get; set; }

        protected RepositoryTests()
        {
            var mgc = new Mock<IRawGraphClient>();
            mgc.Setup(gc => gc.Cypher).Returns(new CypherFluentQuery(mgc.Object));
            mgc.Setup(gc => gc.CypherCapabilities).Returns(CypherCapabilities.Cypher30);
            MockGraphClient = mgc;
            Data = new List<TEntity>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Repository?.Dispose();
            }
        }

        protected void SetData(List<TEntity> newData)
        {
            Data.Clear();
            Data.AddRange(newData);
        }

        protected void VerifyQueryWithResults(string cypherQuery, Dictionary<string, object> parameters = null)
        {
            VerifyQueryWithResults<TEntity>(cypherQuery, parameters);
        }

        protected void VerifyQueryWithResults<TQuery>(string cypherQuery, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            var expectedQuery = new CypherQuery(cypherQuery, parameters, CypherResultMode.Projection);
            MockGraphClient.Verify(gc => gc.ExecuteGetCypherResultsAsync<TQuery>(It.Is<CypherQuery>((actual) => CompareQuery(expectedQuery, actual, true))), Times.Once);
        }

        protected void VerifyQuery(string cypherQuery, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            var expectedQuery = new CypherQuery(cypherQuery, parameters, CypherResultMode.Projection);
            MockGraphClient.Verify(gc => gc.ExecuteCypherAsync(It.Is<CypherQuery>((actual) => CompareQuery(expectedQuery, actual, true))), Times.Once);
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

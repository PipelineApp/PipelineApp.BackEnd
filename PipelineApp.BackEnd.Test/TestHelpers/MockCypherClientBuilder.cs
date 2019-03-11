// <copyright file="MockCypherClientBuilder.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Interfaces.Data;
    using Moq;
    using Neo4jClient;
    using Neo4jClient.Cypher;

    public class MockCypherClientBuilder<TModel>
        where TModel : IEntity
    {
        private readonly Mock<ICypherFluentQuery> _mockCypher;
        private readonly Mock<ICypherFluentQuery> _mockMatch;
        private readonly Mock<IGraphClient> _mockGraphClient;

        public MockCypherClientBuilder()
        {
            _mockCypher = new Mock<ICypherFluentQuery>();
            _mockMatch = new Mock<ICypherFluentQuery>();
            _mockGraphClient = new Mock<IGraphClient>();
        }

        public MockCypherClientBuilder<TModel> WithMatch(string matchQuery)
        {
            _mockCypher
                .Setup(x => x.Match(matchQuery))
                .Returns(_mockMatch.Object);
            return this;
        }

        public MockCypherClientBuilder<TModel> WithReturnSingle(Expression<Func<ICypherResultItem, TModel>> query, TModel value)
        {
            IEnumerable<TModel> mockReturnsResult = new List<TModel> { value };
            var mockReturn = new Mock<ICypherFluentQuery<TModel>>();

            _mockMatch
                .Setup(x => x.Return(It.Is<Expression<Func<ICypherResultItem, TModel>>>(e => e.ToString() == query.ToString())))
                .Returns(mockReturn.Object);
            mockReturn
                .Setup(r => r.ResultsAsync)
                .Returns(Task.FromResult(mockReturnsResult));

            return this;
        }

        public MockCypherClientBuilder<TModel> WithReturnEnumerable(Expression<Func<ICypherResultItem, TModel>> query, IEnumerable<TModel> value)
        {
            var mockReturn = new Mock<ICypherFluentQuery<TModel>>();

            _mockMatch
                .Setup(x => x.Return(It.Is<Expression<Func<ICypherResultItem, TModel>>>(e => e.ToString() == query.ToString())))
                .Returns(mockReturn.Object);
            mockReturn
                .Setup(r => r.ResultsAsync)
                .Returns(Task.FromResult(value));

            return this;
        }

        public MockCypherClientBuilder<TModel> WithWhere(Expression<Func<TModel, bool>> predicate)
        {
            _mockMatch
                .Setup(x => x.Where(It.Is<Expression<Func<TModel, bool>>>(e => e.ToString() == predicate.ToString())))
                .Returns(_mockMatch.Object);
            return this;
        }

        public Mock<IGraphClient> Finalize()
        {
            _mockGraphClient
                .Setup(x => x.Cypher)
                .Returns(_mockCypher.Object);
            return _mockGraphClient;
        }
    }
}

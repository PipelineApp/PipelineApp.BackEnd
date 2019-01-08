// <copyright file="BaseRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data
{
    using System;
    using Neo4j.Driver.V1;
    using Providers;

    /// <summary>
    /// Base class for all repository classes, which initializes graph client
    /// and manages disposal.
    /// </summary>
    public class BaseRepository : IDisposable
    {
        protected readonly ISession Session;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="graphDriver">The graph driver.</param>
        public BaseRepository(IDriver graphDriver)
        {
            Session = graphDriver.Session();
        }

        public int Count(string vertexType)
        {
            var query = $"MATCH (v:{vertexType}) RETURN count(*)";
            var data = Session.Run(query);
            return data.Peek().GetOrDefault("count(*)", 0);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Session.CloseAsync();
            }
        }
    }
}

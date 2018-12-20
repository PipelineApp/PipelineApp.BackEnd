// <copyright file="DatabaseSeeder.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Seeders
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Data;
    using Interfaces;

    /// <summary>
    /// Seeder class used to initialize database content for a blank database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DatabaseSeeder
    {
        private readonly IGraphDbClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSeeder"/> class.
        /// </summary>
        /// <param name="client">The graph DB client.</param>
        public DatabaseSeeder(IGraphDbClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Seeds the database with fandom vertices.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        public async Task Seed()
        {
            await InitFandoms();
        }

        private async Task InitFandoms()
        {
            var initialized = _client.Count(VertexTypes.FANDOM) > 0;
            if (initialized)
            {
                return;
            }
            await _client.CreateFandom("Star Trek");
            await _client.CreateFandom("Mass Effect");
            await _client.CreateFandom("Dragon Age");
        }
    }
}

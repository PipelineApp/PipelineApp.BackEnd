// <copyright file="IGraphDbClient.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Data.Entities;

    /// <summary>
    /// Client class for interactions with a graph database.
    /// </summary>
    public interface IGraphDbClient : IDisposable
    {
        /// <summary>
        /// Inserts a new fandom object into the database.
        /// </summary>
        /// <param name="name">The name of the fandom to be created.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the created fandom.
        /// </returns>
        Task<Fandom> CreateFandom(string name);

        /// <summary>
        /// Retrieves the number of nodes in the database of the given type.
        /// </summary>
        /// <param name="vertexType">The vertex type to be counted.</param>
        /// <returns>The number of matching nodes in the database.</returns>
        int Count(string vertexType);

        /// <summary>
        /// Retrieves all vertices in the database of the given type.
        /// </summary>
        /// <typeparam name="T">The type to which the retrieved nodes should be mapped.</typeparam>
        /// <param name="vertexType">The vertex type whose nodes should be retrieved.</param>
        /// <returns>A collection of objects of type <code>T</code> retrieved from the database.</returns>
        IEnumerable<T> GetAll<T>(string vertexType)
            where T : GraphEntity, new();
    }
}

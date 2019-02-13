// <copyright file="IRepository.cs" company="Blackjack Software">
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
    /// <typeparam name="T">The type to which query results should be cast.</typeparam>
    public interface IRepository<T> : IDisposable
        where T : GraphEntity
    {
        /// <summary>
        /// Inserts a new object of type <code>T</code> into the database.
        /// </summary>
        /// <param name="data">The object to be inserted.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the created object.
        /// </returns>
        Task<T> Create(T data);

        /// <summary>
        /// Retrieves the number of nodes in the database of type <code>T</code> associated with the given user ID.
        /// </summary>
        /// <param name="userId">The user whose associated vertices should be counted.</param>
        /// <returns>The number of matching nodes in the database.</returns>
        int Count(string userId);

        /// <summary>
        /// Retrieves all vertices in the database of type <code>T</code> associated with the given user ID.
        /// </summary>
        /// <param name="userId">The user whose associated vertices should be searched for.</param>
        /// <returns>A collection of objects of type <code>T</code> retrieved from the database.</returns>
        Task<IEnumerable<T>> GetAll(string userId);

        /// <summary>
        /// Fetches a vertex of type <code>T</code> with the given unique identifier.
        /// </summary>
        /// <param name="id">The identifier to search by.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the matching vertex of type T, or null if no vertex
        /// exists with the given ID.
        /// </returns>
        Task<T> GetById(string id);
    }
}

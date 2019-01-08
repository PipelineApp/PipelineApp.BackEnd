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
        /// Retrieves the number of nodes in the database of type <code>T</code>.
        /// </summary>
        /// <returns>The number of matching nodes in the database.</returns>
        int Count();

        /// <summary>
        /// Retrieves all vertices in the database of type <code>T</code>.
        /// </summary>
        /// <typeparam name="T">The type to which the retrieved nodes should be mapped.</typeparam>
        /// <returns>A collection of objects of type <code>T</code> retrieved from the database.</returns>
        IEnumerable<T> GetAll();
    }
}

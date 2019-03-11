// <copyright file="IRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data;
    using Infrastructure.Data.Entities;
    using Infrastructure.Data.Relationships;

    /// <summary>
    /// Base repository for CRUD operations relating to the given model class.
    /// </summary>
    /// <typeparam name="TModel">Model class on which CRUD operations should be performed.</typeparam>
    public interface IRepository<TModel> : IDisposable
        where TModel : IEntity
    {
        /// <summary>
        /// Gets all values in the database of the given type.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains a list of <code>TModel</code>
        /// objects.
        /// </returns>
        Task<IList<TModel>> GetAllAsync();

        /// <summary>
        /// Gets a single object of class <code>TModel</code> by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the object to be fetched.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the retrieved object.
        /// </returns>
        Task<TModel> GetByIdAsync(Guid id);

        /// <summary>
        /// Inserts a new object into the database.
        /// </summary>
        /// <param name="model">The object to be inserted.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the created object.
        /// </returns>
        Task<TModel> SaveAsync(TModel model);

        /// <summary>
        /// Inserts a new object into the database, as well as a relationship of type <code>TRelationship</code>
        /// from another node (of type <code>TSource</code> with ID <code>sourceId</code>) to the created node.
        /// </summary>
        /// <typeparam name="TRelationship">The type of the relationship edge to be created.</typeparam>
        /// <typeparam name="TSource">The type of the source node for the relationship to be created.</typeparam>
        /// <param name="model">The object to be inserted.</param>
        /// <param name="sourceId">The unique identifier of the source node to which the inserted object should be connected.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the created object.
        /// </returns>
        Task<TModel> CreateWithInboundRelationshipAsync<TRelationship, TSource>(TModel model, Guid sourceId)
            where TRelationship : BaseRelationship
            where TSource : BaseEntity;

        /// <summary>
        /// Updates an existing object in the database.
        /// </summary>
        /// <param name="model">The object with its updated properties.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the updated object.
        /// </returns>
        Task<TModel> UpdateAsync(TModel model);

        /// <summary>
        /// Removes an object and its relationships from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the object to be deleted.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Fetches a count of all objects of type <code>TModel</code>
        /// in the database.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the number of objects in the database.
        /// </returns>
        Task<long> Count();
    }
}

// <copyright file="IPipelineService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Models.DomainModels;
    using Repositories;

    /// <summary>
    /// Service for data manipulation relating to user authentication.
    /// </summary>
    public interface IPipelineService
    {
        /// <summary>
        /// Initializes and saves a new pipeline object.
        /// </summary>
        /// <param name="pipeline">Data regarding the pipeline to be created.</param>
        /// <param name="userId">The unique ID of the user to which the pipeline should be connected.</param>
        /// <param name="repository">The pipeline repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the created pipeline.
        /// </returns>
        Task<Pipeline> CreatePipeline(Pipeline pipeline, Guid? userId, IPipelineRepository repository, IMapper mapper);
    }
}

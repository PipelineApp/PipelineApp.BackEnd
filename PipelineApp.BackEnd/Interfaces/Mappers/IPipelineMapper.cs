// <copyright file="IPipelineMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Mappers
{
    using Infrastructure.Data.Entities;
    using Infrastructure.Data.Entities.EntityCollections;
    using Models.DomainModels;
    using Models.RequestModels.Pipeline;
    using Models.ViewModels;

    /// <summary>
    /// Mapping class for mapping between view model, domain model, and entity representations of pipelines.
    /// </summary>
    public interface IPipelineMapper
    {
        /// <summary>
        /// Maps the given domain model to a DTO.
        /// </summary>
        /// <param name="pipeline">The pipeline domain model.</param>
        /// <returns>A DTO representation of the pipeline.</returns>
        PipelineDto ToDto(Pipeline pipeline);

        /// <summary>
        /// Maps the given request model to a domain layer object.
        /// </summary>
        /// <param name="requestModel">The pipeline request model.</param>
        /// <returns>A domain layer representation of the pipeline.</returns>
        Pipeline ToDomainModel(UpsertPipelineRequestModel requestModel);

        /// <summary>
        /// Maps the given entity data collection to a domain model.
        /// </summary>
        /// <param name="entities">The collection of related pipeline entities.</param>
        /// <returns>A data layer representation of the pipeline.</returns>
        Pipeline ToDomainModel(PipelineEntityDataCollection entities);

        /// <summary>
        /// Maps the given data entity to a domain model.
        /// </summary>
        /// <param name="entity">The pipeline data entity.</param>
        /// <returns>A domain layer representation of the pipeline.</returns>
        Pipeline ToDomainModel(PipelineEntity entity);

        /// <summary>
        /// Maps the given domain model to a data entity.
        /// </summary>
        /// <param name="model">The pipeline domain model.</param>
        /// <returns>A data layer representation of the pipeline.</returns>
        PipelineEntity ToEntity(Pipeline model);
    }
}

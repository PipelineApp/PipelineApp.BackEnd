// <copyright file="PipelineService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Entities;
    using Data.Relationships;
    using Data.Requests;
    using Exceptions.Pipeline;
    using Interfaces.Mappers;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;

    /// <inheritdoc />
    public class PipelineService : IPipelineService
    {
        /// <inheritdoc />
        public Pipeline CreatePipeline(Pipeline pipeline, Guid? userId, IPipelineRepository repository, IPipelineMapper mapper)
        {
            if (userId == null)
            {
                throw new ArgumentException("User ID cannot be null.");
            }
            var entity = mapper.ToEntity(pipeline);
            var request = new CreateNodeRequest<PipelineEntity>(entity)
                .WithInboundRelationshipFrom<ManagesPipeline>(userId.Value);
            var result = repository.CreateWithRelationships(request);
            return mapper.ToDomainModel(result);
        }

        /// <inheritdoc />
        public async Task AssertUserOwnsPipeline(Guid pipelineId, Guid? userId, IPipelineRepository repository)
        {
            if (userId == null)
            {
                throw new ArgumentException("User ID cannot be null.");
            }
            var entities = await repository.GetByUserIdAsync(userId);
            if (entities.All(e => e.Pipeline.Id != pipelineId))
            {
                throw new PipelineNotFoundException();
            }
        }

        /// <inheritdoc />
        public async Task AddTrackedPersona(Guid pipelineId, Guid personaId, IPipelineRepository pipelineRepository, IPipelineMapper mapper)
        {
            await pipelineRepository.AddOutboundRelationshipAsync<Tracks, PersonaEntity>(pipelineId, personaId);
        }

        /// <inheritdoc />
        public async Task AddTrackedFandom(Guid pipelineId, Guid fandomId, IPipelineRepository pipelineRepository, IPipelineMapper mapper)
        {
            await pipelineRepository.AddOutboundRelationshipAsync<Tracks, FandomEntity>(pipelineId, fandomId);
        }

        /// <inheritdoc />
        public async Task<List<Pipeline>> GetAllPipelines(Guid? userId, IPipelineRepository pipelineRepository, IPipelineMapper mapper)
        {
            var result = await pipelineRepository.GetByUserIdAsync(userId);
            return result.Select(mapper.ToDomainModel).ToList();
        }

        /// <inheritdoc />
        public async Task<Pipeline> UpdatePipeline(Pipeline model, IPipelineRepository pipelineRepository, IPipelineMapper mapper)
        {
            var entity = mapper.ToEntity(model);
            var result = await pipelineRepository.UpdateAsync(entity);
            return mapper.ToDomainModel(result);
        }

        /// <inheritdoc />
        public async Task DeletePipeline(Guid pipelineId, IPipelineRepository pipelineRepository)
        {
            await pipelineRepository.DeleteAsync(pipelineId);
        }

        /// <inheritdoc />
        public async Task RemoveTrackedPersona(Guid pipelineId, Guid personaId, IPipelineRepository pipelineRepository)
        {
            await pipelineRepository.RemoveOutboundRelationshipAsync<Tracks, PersonaEntity>(pipelineId, personaId);
        }

        /// <inheritdoc />
        public async Task RemoveTrackedFandom(Guid pipelineId, Guid fandomId, IPipelineRepository pipelineRepository)
        {
            await pipelineRepository.RemoveOutboundRelationshipAsync<Tracks, FandomEntity>(pipelineId, fandomId);
        }
    }
}

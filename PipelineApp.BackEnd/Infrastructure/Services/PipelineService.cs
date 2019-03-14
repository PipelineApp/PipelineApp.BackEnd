// <copyright file="PipelineService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Entities;
    using Data.Relationships;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;

    /// <inheritdoc />
    public class PipelineService : IPipelineService
    {
        /// <inheritdoc />
        public async Task<Pipeline> CreatePipeline(Pipeline pipeline, Guid? userId, IPipelineRepository repository, IMapper mapper)
        {
            if (userId == null)
            {
                throw new ArgumentException("User ID cannot be null.");
            }
            var entity = mapper.Map<PipelineEntity>(pipeline);
            var result = await repository.CreateWithInboundRelationshipAsync<Manages, UserEntity>(entity, userId.Value);
            return mapper.Map<Pipeline>(result);
        }
    }
}

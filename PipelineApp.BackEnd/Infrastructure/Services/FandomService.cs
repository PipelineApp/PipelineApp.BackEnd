// <copyright file="FandomService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Data;
    using Interfaces;
    using Interfaces.Services;
    using Models.DomainModels;

    /// <inheritdoc />
    public class FandomService : IFandomService
    {
        /// <inheritdoc />
        public IEnumerable<Fandom> GetAllFandoms(IGraphDbClient client, IMapper mapper)
        {
            var fandomEntities = client.GetAll<Data.Entities.Fandom>(VertexTypes.FANDOM);
            return fandomEntities.Select(mapper.Map<Fandom>).ToList();
        }
    }
}

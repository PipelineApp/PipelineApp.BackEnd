// <copyright file="FandomService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces.Mappers;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;

    /// <inheritdoc />
    public class FandomService : IFandomService
    {
        /// <inheritdoc />
        public async Task<IEnumerable<Fandom>> GetAllFandoms(IFandomRepository repository, IFandomMapper mapper)
        {
            var fandomEntities = await repository.GetAllAsync();
            return fandomEntities.Select(mapper.ToDomainModel).ToList();
        }
    }
}

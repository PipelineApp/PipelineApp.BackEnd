// <copyright file="FandomMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Mappers
{
    using Data.Entities;
    using Interfaces.Mappers;
    using Models.DomainModels;
    using Models.ViewModels;

    /// <inheritdoc />
    public class FandomMapper : IFandomMapper
    {
        /// <inheritdoc />
        public FandomDto ToDto(Fandom fandom)
        {
            return new FandomDto
            {
                Id = fandom.Id,
                Name = fandom.Name
            };
        }

        /// <inheritdoc />
        public Fandom ToDomainModel(FandomEntity entity)
        {
            return new Fandom
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}

// <copyright file="IFandomMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Mappers
{
    using Infrastructure.Data.Entities;
    using Models.DomainModels;
    using Models.ViewModels;

    /// <summary>
    /// Mapping class for mapping between view model, domain model, and entity representations of fandoms.
    /// </summary>
    public interface IFandomMapper
    {
        /// <summary>
        /// Maps the given domain model to a DTO.
        /// </summary>
        /// <param name="fandom">The fandom domain model.</param>
        /// <returns>A DTO representation of the fandom.</returns>
        FandomDto ToDto(Fandom fandom);

        /// <summary>
        /// Maps the given entity to a domain model.
        /// </summary>
        /// <param name="entity">The fandom data entity.</param>
        /// <returns>A domain-layer representation of the fandom.</returns>
        Fandom ToDomainModel(FandomEntity entity);
    }
}

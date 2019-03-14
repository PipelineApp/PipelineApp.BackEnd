// <copyright file="Fandom.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.DomainModels
{
    using System;
    using Infrastructure.Data.Entities;
    using ViewModels;

    /// <summary>
    /// Domain-layer representation of a fandom.
    /// </summary>
    public class Fandom
    {
        /// <summary>
        /// Gets or sets the unique identifier for this fandom.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the fandom's name.
        /// </summary>
        public string Name { get; set; }

        public Fandom(FandomDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
        }

        public Fandom(FandomEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }

        public FandomEntity ToEntity()
        {
            var entity = new FandomEntity();
            if (Id != null)
            {
                entity.Id = Id.Value;
            }

            entity.Name = Name;
            return entity;
        }

        public FandomDto ToDto()
        {
            return new FandomDto
            {
                Id = Id,
                Name = Name
            };
        }
    }
}

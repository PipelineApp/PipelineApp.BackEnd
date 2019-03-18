// <copyright file="RoleEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    /// <summary>
    /// Data-layer representation of a user's role.
    /// </summary>
    public class RoleEntity : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEntity"/> class.
        /// </summary>
        public RoleEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEntity"/> class.
        /// </summary>
        /// <param name="name">The role's name.</param>
        public RoleEntity(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the role's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the role's normalized name.
        /// </summary>
        public string NormalizedName { get; set; }
    }
}

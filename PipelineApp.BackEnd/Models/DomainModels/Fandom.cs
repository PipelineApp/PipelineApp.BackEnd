// <copyright file="Fandom.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.DomainModels
{
    using System;

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
    }
}

// <copyright file="FandomDto.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.ViewModels
{
    using System;

    /// <summary>
    /// View model representation of a fandom.
    /// </summary>
    public class FandomDto
    {
        /// <summary>
        /// Gets or sets the fandom's unique identifier.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the fandom's name.
        /// </summary>
        public string Name { get; set; }
    }
}

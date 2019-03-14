// <copyright file="PipelineDto.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// View model representation of a pipeline.
    /// </summary>
    public class PipelineDto
    {
        /// <summary>
        /// Gets or sets the pipeline's unique identifier.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the pipeline's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the pipeline's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the fandoms tracked by the pipeline.
        /// </summary>
        public List<FandomDto> Fandoms { get; set; }

        /// <summary>
        /// Gets or sets the personas tracked by the pipeline.
        /// </summary>
        public List<PersonaDto> Personas { get; set; }
    }
}

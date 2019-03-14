// <copyright file="PipelineEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    /// <summary>
    /// Data-layer representation of a pipeline.
    /// </summary>
    public class PipelineEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the pipeline's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the pipeline's description.
        /// </summary>
        public string Description { get; set; }
    }
}

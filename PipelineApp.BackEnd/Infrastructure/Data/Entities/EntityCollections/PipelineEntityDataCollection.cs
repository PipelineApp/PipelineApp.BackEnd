// <copyright file="PipelineEntityDataCollection.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities.EntityCollections
{
    using System.Collections.Generic;

    /// <summary>
    /// Container class for data-layer representations of a pipeline and the
    /// entities it tracks.
    /// </summary>
    public class PipelineEntityDataCollection
    {
        /// <summary>
        /// Gets or sets the pipeline.
        /// </summary>
        public PipelineEntity Pipeline { get; set; }

        /// <summary>
        /// Gets or sets the fandoms tracked by the pipeline.
        /// </summary>
        public List<FandomEntity> Fandoms { get; set; }

        /// <summary>
        /// Gets or sets the personas tracked by the pipeline.
        /// </summary>
        public List<PersonaEntity> Personas { get; set; }
    }
}

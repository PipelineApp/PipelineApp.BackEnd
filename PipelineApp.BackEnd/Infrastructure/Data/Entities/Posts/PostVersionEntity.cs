// <copyright file="PostVersionEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using System;

    /// <summary>
    /// Data-layer represenation of a particular version of a post's content.
    /// </summary>
    public class PostVersionEntity : VersionEntity<PostEntity>
    {
        /// <summary>
        /// Gets or sets the post version's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the post version's content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the post version's fandom ID
        /// </summary>
        public Guid FandomId { get; set; }
    }
}

// <copyright file="PostEntityDataCollection.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities.EntityCollections
{
    using System.Collections.Generic;

    /// <summary>
    /// Container class for data-layer representations of a post and its relationships and versions
    /// </summary>
    public class PostEntityDataCollection
    {
        /// <summary>
        /// Gets or sets the post entity.
        /// </summary>
        public PostEntity Post { get; set; }

        /// <summary>
        /// Gets or sets the list of the post's versions.
        /// </summary>
        public List<PostVersionEntity> Versions { get; set; }

        /// <summary>
        /// Gets or sets the previous post in the post's thread.
        /// </summary>
        public PostEntity ParentPost { get; set; }

        /// <summary>
        /// Gets or sets the root post in the post's thread.
        /// </summary>
        public PostEntity RootPost { get; set; }
    }
}

// <copyright file="Post.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.DomainModels
{
    using System;

    /// <summary>
    /// Domain model representing a post.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Gets or sets the post's unique identifier.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the post's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the post's content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the post which this post was shared from.
        /// </summary>
        public Post ParentPost { get; set; }

        /// <summary>
        /// Gets or sets the root post of the thread to which the post belongs.
        /// </summary>
        public Post RootPost { get; set; }
    }
}

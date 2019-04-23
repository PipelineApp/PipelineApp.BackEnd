// <copyright file="BaseRelationship.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Relationships
{
    using System;

    public abstract class BaseRelationship
    {
        public Guid SourceId { get; set; }

        public Guid TargetId { get; set; }
    }
}

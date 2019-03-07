// <copyright file="MockChildController.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.TestHelpers
{
    using BackEnd.Controllers;

    public class MockChildController : BaseController
    {
        public string RetrieveUserId()
        {
            return UserId;
        }
    }
}

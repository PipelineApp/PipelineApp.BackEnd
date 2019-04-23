// <copyright file="PipelineController.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Exceptions.Pipeline;
    using Infrastructure.Exceptions.Post;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.DomainModels;
    using Models.RequestModels.Pipeline;
    using Models.RequestModels.Post;
    using Models.ViewModels;
    using Newtonsoft.Json;

    /// <summary>
    /// Controller class for behavior related to post data.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PostController : BaseController
    {
        private readonly ILogger<PostController> _logger;
        private readonly IPersonaService _personaService;
        private readonly IPersonaRepository _personaRepository;
        private readonly IPostService _postService;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostController"/> class.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="personaService">The persona service.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <param name="postService">The post service.</param>
        /// <param name="postRepository">The pipeline repository.</param>
        /// <param name="mapper">The mapper.</param>
        public PostController(
            ILogger<PostController> logger,
            IPersonaService personaService,
            IPersonaRepository personaRepository,
            IPostService postService,
            IPostRepository postRepository,
            IMapper mapper)
        {
            _logger = logger;
            _personaService = personaService;
            _personaRepository = personaRepository;
            _postService = postService;
            _postRepository = postRepository;
            _mapper = mapper;
        }
    }
}

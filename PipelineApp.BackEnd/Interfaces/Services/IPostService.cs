namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Models.DomainModels;
    using Repositories;

    public interface IPostService
    {
        Post CreateBasePost(Post post, Guid personaId, Guid fandomId, IPostRepository postRepository, IMapper mapper);
    }
}

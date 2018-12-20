namespace PipelineApp.BackEnd.Interfaces
{
    using System.Collections.Generic;
    using AutoMapper;
    using Models.DomainModels;

    public interface IFandomService
    {
        IEnumerable<Fandom> GetAllFandoms(IGraphDbClient graphDbClient, IMapper mapper);
    }
}

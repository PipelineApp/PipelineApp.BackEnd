namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Data;
    using Interfaces;
    using Models.DomainModels;

    public class FandomService : IFandomService
    {
        public IEnumerable<Fandom> GetAllFandoms(IGraphDbClient client, IMapper mapper)
        {
            var fandomEntities = client.GetAll<Data.Entities.Fandom>(VertexTypes.FANDOM);
            return fandomEntities.Select(mapper.Map<Fandom>).ToList();
        }
    }
}

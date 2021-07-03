using System.Linq;
using AutoMapper;
using Contracts.DAL.Base.Mappers;
using Domain.Base;

namespace DAL.APP.EF.Mappers
{
    public class BaseMapper<TOutEntity, TInEntity> : IBaseMapper<TOutEntity, TInEntity>
    {
        protected readonly IMapper Mapper;

        public BaseMapper(IMapper mapper)
        {
            Mapper = mapper;
        }

        public virtual TOutEntity Map(TInEntity? inEntity)
        {
            // var mappedEntity = Mapper.Map<TOutEntity>(inEntity);
            // var translations = mappedEntity?
            //     .GetType()
            //     .GetProperties()
            //     .Where(x => x.PropertyType == typeof(LangString));
            // if (translations != null)
            // {
            //     foreach (var translationProperty in translations)
            //     {
            //         var langString = (LangString) translationProperty.GetValue(inEntity)!;
            //         foreach (var translation in langString.Translations ?? Enumerable.Empty<Translation>())
            //         {
            //             // translation.Value = 
            //         }
            //     }
            // }
            //
            //
            // return mappedEntity;
            return Mapper.Map<TOutEntity>(inEntity);

        }
        // Dal -> domain
        public virtual TInEntity Map(TOutEntity? inEntity)
        {
            return Mapper.Map<TInEntity>(inEntity);
        }
    }
}
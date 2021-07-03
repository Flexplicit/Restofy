using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using DAL.App.DTO.OrderModels;
using Domain.Base;

namespace DAL.APP.EF.Mappers
{
    public class RestaurantMapper : BaseMapper<DAL.App.DTO.OrderModels.Restaurant, Domain.OrderModels.Restaurant>
    {
        public RestaurantMapper(IMapper mapper) : base(mapper)
        {
        }

        public override Restaurant Map(Domain.OrderModels.Restaurant? inEntity)
        {
            return base.Map(inEntity);
        }


        /*
         Works fine for smaller applications, probably need a better translating system for bigger ones when we have
         thousands of words and 10+ languages
        */
        // public override Domain.OrderModels.Restaurant Map(Restaurant? inEntity)
        // {
        //     var res = base.Map(inEntity);
        //
        //     res.NameLang!.Id = inEntity!.NameLangId;
        //     foreach (var nameTranslation in res.NameLang.Translations!)
        //     {
        //         nameTranslation.LangStringId = inEntity.NameLangId;
        //     }
        //
        //     if (res.DescriptionLangId == null)
        //     {
        //         return res;
        //     }
        //     
        //     //new language w value added if not not/null is default
        //     res.DescriptionLang!.Id = inEntity.DescriptionLangId!.Value;
        //     foreach (var descriptionTranslation in res.DescriptionLang?.Translations ?? Enumerable.Empty<Translation>())
        //     {
        //         descriptionTranslation.Value = inEntity.DescriptionLang!;
        //     }
        //
        //
        //     return res;
        // }
    }
}
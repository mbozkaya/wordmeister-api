using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Interfaces;

namespace wordmeister_api.Helpers
{
    public class Mapper :IMapper
    {
        public Mapper()
        {
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return source.Adapt<TDestination>();
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeWriterSample
{
    public class Resolver : DefaultContractResolver
    {
        #region variables

        #endregion

        #region properties

        #endregion

        #region construction

        #endregion

        #region methods
        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);
            if (ConverterCache.Converters.TryGetValue(objectType, out JsonConverter converter))
                contract.Converter = converter;

            return contract;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            //return base.CreateProperties(type, memberSerialization).ToList();
            return base.CreateProperties(type, memberSerialization).Where(p => p.Writable).ToList();
        }
        #endregion

        #region event methods

        #endregion

        #region overrides

        #endregion
    }
}

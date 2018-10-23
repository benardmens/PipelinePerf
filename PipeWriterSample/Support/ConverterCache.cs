using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PipeWriterSample
{
    internal static class ConverterCache
    {
        #region variables

        #endregion

        #region properties
        internal static Dictionary<Type, JsonConverter> Converters { get; }
        internal static List<JsonConverter> CreationConverters { get; }
        #endregion

        #region construction
        static ConverterCache()
        {
            var types = Assembly.GetAssembly(typeof(ConverterCache)).GetTypes();
            Converters = new Dictionary<Type, JsonConverter>();
            var converterTypes = types.
                Where(x => !x.IsAbstract && !x.IsInterface && typeof(IConverterDescriptor).IsAssignableFrom(x)).
                ToArray();

            foreach (var converterDescriptor in converterTypes.Select(x => Activator.CreateInstance(x) as IConverterDescriptor).Where(x => x != null))
            {
                Converters[converterDescriptor.TypeToConvert] = converterDescriptor as JsonConverter;
            }

            CreationConverters = types.
                Where(x => typeof(JsonConverter).
                IsAssignableFrom(x)).
                Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(CustomCreationConverter<>)).
                Select(x => Activator.CreateInstance(x) as JsonConverter).
                ToList();
        }
        #endregion

        #region methods

        #endregion

        #region event methods

        #endregion

        #region overrides

        #endregion
    }
}

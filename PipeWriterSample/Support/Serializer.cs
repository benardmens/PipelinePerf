using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace PipeWriterSample
{
    public static class Serializer
    {
        #region variables

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ContractResolver = new Resolver(),
            Converters = ConverterCache.CreationConverters,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        #endregion

        #region properties

        #endregion

        #region construction

        static Serializer()
        {
            Settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        }

        #endregion

        #region methods

        /// <summary>
        /// Deserializes a json string into the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content, Settings);
        }

        /// <summary>
        /// Serializes object into a json string
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        public static string Serialize(object subject, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(subject, formatting, Settings);
        }

        #endregion

        #region event methods

        #endregion

        #region overrides

        #endregion
    }
}

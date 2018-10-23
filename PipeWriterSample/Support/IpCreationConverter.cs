using Newtonsoft.Json.Converters;
using System;
using System.Net;

namespace PipeWriterSample
{
    public class IpCreationConverter : CustomCreationConverter<IPAddress>
    {
        #region variables

        #endregion

        #region properties

        #endregion

        #region construction

        #endregion

        #region methods

        #endregion

        #region event methods

        #endregion

        #region overrides
        public override bool CanConvert(Type objectType)
        {
            var retVal = objectType == typeof(IPAddress);
            if (retVal)
                return true;
            return objectType == typeof(IPAddress);
        }

        public override IPAddress Create(Type objectType)
        {
            return IPAddress.Parse("0.0.0.0");
        }
        #endregion
    }
}

using Newtonsoft.Json.Converters;
using System;
using System.Net;

namespace PipeWriterSample
{
    public class IpEndPointCreationConverter : CustomCreationConverter<IPEndPoint>
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
            var retVal = objectType == typeof(IPEndPoint);
            if (retVal)
                return true;
            return objectType == typeof(IPEndPoint);
        }

        public override IPEndPoint Create(Type objectType)
        {
            return new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
        }
        #endregion
    }
}

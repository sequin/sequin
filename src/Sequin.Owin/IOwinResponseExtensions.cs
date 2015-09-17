namespace Sequin.Owin
{
    using System.Net;
    using Microsoft.Owin;
    using Newtonsoft.Json;

    public static class IOwinResponseExtensions
    {
        public static void BadRequest(this IOwinResponse response, string message)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ReasonPhrase = message;
        }

        public static void Json(this IOwinResponse response, object body)
        {
            response.ContentType = "application/json";

            response.StatusCode = (int)HttpStatusCode.OK;
            response.Write(JsonConvert.SerializeObject(body));
        }
    }
}

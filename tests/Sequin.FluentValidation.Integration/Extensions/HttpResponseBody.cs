namespace Sequin.FluentValidation.Integration.Extensions
{
    using System.Net.Http;
    using Jil;

    internal static class HttpResponseBody
    {
        public static string Body(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }

        public static T BodyAs<T>(this HttpResponseMessage response)
        {
            var body = response.Body();
            return JSON.Deserialize<T>(body);
        }
    }
}

namespace Sequin.Integration.Features
{
    using System.Net;
    using System.Net.Http;
    using Extensions;
    using FluentAssertions;
    using Xbehave;

    public class OptionsHttpMethodFeature : FeatureBase
    {
        [Scenario, Example("/commands"), Example("/commands/mycommand")]
        public void OptionsRequestToAnyCommandUrl(string url, HttpResponseMessage response)
        {
            "When I issue an HTTP OPTIONS request"
                .When(async () =>
                {
                    response = await Server.CreateRequest(url).SendAsync("OPTIONS");
                });

            "Then the response should return OK"
                .Then(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                });

            "And the Allow header should be set"
                .And(() =>
                {
                    response.Content.Headers.Allow.Should().Contain(new[] {"PUT", "OPTIONS"});
                });

            "And the response body should be empty"
                .And(() =>
                {
                    response.Body().Should().BeEmpty();
                });
        }

        [Scenario]
        public void OptionsRequestToAnyNonCommandUrl(HttpResponseMessage response)
        {
            "When I issue an HTTP OPTIONS request"
                .When(async () =>
                {
                    response = await Server.CreateRequest("/any/random/url").SendAsync("OPTIONS");
                });

            "Then the request should not have been handled by Sequin"
                .Then(() =>
                {
                    // If there is no other middleware the response would be Not Found
                    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                });
        }
    }
}
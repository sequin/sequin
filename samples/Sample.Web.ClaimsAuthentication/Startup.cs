namespace Sample.Web.ClaimsAuthentication
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Microsoft.Owin;
    using Owin;
    using Sequin;
    using Sequin.ClaimsAuthentication.Middleware;
    using Sequin.Infrastructure;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            // Force sign-in with role
            app.Use(new Func<AppFunc, AppFunc>(next => (async env =>
                                                              {
                                                                  var context = new OwinContext(env);
                                                                  var claims = new List<Claim>
                                                                               {
                                                                                   new Claim(ClaimTypes.Role, "SomeRole")
                                                                               };

                                                                  context.Request.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "SomeAuthType"));

                                                                  await next.Invoke(env);
                                                              })));

            app.UseSequin(new SequinOptions
                          {
                              CommandPipeline = new []
                                                {
                                                    new CommandPipelineStage(typeof(OptInCommandAuthorization))
                                                }
                          });
        }
    }
}
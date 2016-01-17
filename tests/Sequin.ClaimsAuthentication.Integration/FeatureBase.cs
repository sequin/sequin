namespace Sequin.ClaimsAuthentication.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Fakes;
    using Infrastructure;
    using Microsoft.Owin;
    using Microsoft.Owin.Testing;
    using Xbehave;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public abstract class FeatureBase
    {
        private bool isSignedIn;
        private List<string> userRoles;
        private CommandTrackingPostProcessor postProcessor;

        protected Type AuthorizationMiddleware { get; set; }
        protected TestServer Server { get; private set; }

        [Background]
        public void Background()
        {
            isSignedIn = false;
            userRoles = new List<string>();
            postProcessor = new CommandTrackingPostProcessor();

            Server = TestServer.Create(app =>
                                       {
                                           app.Use(new Func<AppFunc, AppFunc>(next => (async env =>
                                                                                             {
                                                                                                 if (isSignedIn)
                                                                                                 {
                                                                                                     var context = new OwinContext(env);
                                                                                                     var claims = userRoles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
                                                                                                     context.Request.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "SomeAuthType"));
                                                                                                 }

                                                                                                 await next.Invoke(env);
                                                                                             })));

                                           app.UseSequin(new SequinOptions
                                                         {
                                                            PostProcessor = postProcessor,
                                                            CommandPipeline = new []
                                                                              {
                                                                                  new CommandPipelineStage(AuthorizationMiddleware)
                                                                              }
                                                         });
                                       });
        }

        protected void IsAuthenticatedUser(params string[] roles)
        {
            isSignedIn = true;
            userRoles = roles.ToList();
        }

        protected void IsAnonymousUser()
        {
            isSignedIn = false;
            userRoles = new List<string>();
        }

        protected bool HasExecuted(string commandName)
        {
            return postProcessor.ExecutedCommands.ContainsKey(commandName);
        }
    }
}
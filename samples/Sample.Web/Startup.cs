﻿namespace Sample.Web
{
    using Owin;
    using Sequin.Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSequin();
        }
    }
}
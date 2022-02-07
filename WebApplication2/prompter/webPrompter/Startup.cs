using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.IO;

//[assembly: OwinStartup(typeof(webPrompter.Startup1))]

[assembly: OwinStartup("SignalRChat", typeof(webPrompter.Startup))]

namespace webPrompter
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Дополнительные сведения о настройке приложения см. на странице https://go.microsoft.com/fwlink/?LinkID=316888
            app.Use((context, next) =>
            {
                TextWriter output = context.Get<TextWriter>("host.TraceOutput");
                return next().ContinueWith(result =>
                {
                    output.WriteLine("Scheme {0} : Method {1} : Path {2} : MS {3}",
                    context.Request.Scheme, context.Request.Method, context.Request.Path, getTime());
                });
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync(getTime() + " My First OWIN App");
            });
        }
        string getTime()
        {
            return DateTime.Now.Millisecond.ToString();
        }
    }
}

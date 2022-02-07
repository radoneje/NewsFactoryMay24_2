using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;




namespace webPrompter
{
    public class SignalRChat
    {
       
        public void Configuration(IAppBuilder app)
        {
            // Дополнительные сведения о настройке приложения см. на странице https://go.microsoft.com/fwlink/?LinkID=316888

            app.MapSignalR();// "/webPrompter", new Microsoft.AspNet.SignalR.HubConfiguration() {
                             //    EnableDetailedErrors = true,
                             // EnableJavaScriptProxies=true
                             // });
            System.IO.File.AppendAllText("c:\\tmp\\1.txt", DateTime.Now.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;
using System.Web.WebSockets;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
//using Microsoft.Web.WebSockets;


namespace WebApplication2.handlers
{
  /*  public class MyWSHandler :  WebSocketHandler
    {
        public override void OnOpen()
        {
            this.Send("Welcom from " + this.WebSocketContext.UserHostAddress);
        }
        public override void OnMessage(string message)
        {
            string msgBack = string.Format(
                "You have sent {0} at {1}", message, DateTime.Now.ToLongTimeString());
            this.Send(msgBack);
        }
        public override void OnClose()
        {
            base.OnClose();
        }
        public override void OnError()
        {
            base.OnError();
        }
    }*/
        /// <summary>
        /// Summary description for GetFile
        /// </summary>
        public class WebSocket : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(webSocketRequest);
            }
            else {
                context.Response.ContentType = "text/plain";
                context.Response.Write("This is WEB SOCKET handler");
            }

        }

        private static readonly List<System.Net.WebSockets.WebSocket> clients = new List<System.Net.WebSockets.WebSocket>();
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim(); 


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private async Task webSocketRequest(AspNetWebSocketContext context) {

            
            var socket = context.WebSocket;

            Locker.EnterWriteLock();
            try
            {
                clients.Add(socket);
            }
            finally {
                Locker.ExitWriteLock();   
            }
            while (true) {
                var buffer = new ArraySegment<byte>(new byte[1024]);
                var res = await socket.ReceiveAsync(buffer, CancellationToken.None);

                for (int i= 0; i < clients.Count(); i++)
                {
                    var client = clients[i];
                    try
                    {
                        if (client.State == System.Net.WebSockets.WebSocketState.Open)
                        {
                            await client.SendAsync(buffer, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

                        }

                    }
                    catch (ObjectDisposedException) {
                        Locker.EnterWriteLock();
                        try
                        {
                            clients.Remove(client);
                            i--;
                        }
                        finally {
                            Locker.ExitWriteLock();
                        }

                    }

                }

            }
           
           

        }


    }
    public class WebSocketRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new WebSocket() { RequestContext = requestContext }; ;
        }
    }
   
}

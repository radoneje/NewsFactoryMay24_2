using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebSockets;
using System.Net.WebSockets;

namespace WebApplication2
{
    /// <summary>
    /// Сводное описание для ws
    /// </summary>
    public class ws : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpContext currentContext = HttpContext.Current;
            if (currentContext.IsWebSocketRequest ||
            currentContext.IsWebSocketRequestUpgrading)
            {
                try
                {
                    context.AcceptWebSocketRequest(webSocketRequest);
                }
                catch (Exception e) {
                    var b = e;
                }
              //  context.Response.Status = System.Net.HttpStatusCode.SwitchingProtocols.ToString();
               // context.Response.End();
               
                // return currentContext.Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Привет всем!");
            }
        }
        private static readonly List<System.Net.WebSockets.WebSocket> Clients = new List<System.Net.WebSockets.WebSocket>();
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        private async Task webSocketRequest(System.Web.WebSockets.AspNetWebSocketContext context1)
        {
            var socket = context1.WebSocket;
            Locker.EnterWriteLock();
            try
            {
                Clients.Add(socket);
            }
            finally
            {
                Locker.ExitWriteLock();
            }


            while (true)
            {
                // MUST read if we want the state to get updated...
                var buffer = new ArraySegment<byte>(new byte[100 * 1024]);//100k

                // Ожидаем данные от него
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (result.Count > 1024)
                {
                    var d = 4;
                    var e = d;
                }
                for (int i = 0; i < Clients.Count; i++)
                {

                    var client = Clients[i];
                    var buffer2 = new ArraySegment<byte>(buffer.Take(result.Count).ToArray());

                    try
                    {
                        if (client.State == WebSocketState.Open && client != socket)
                        {
                            await client.SendAsync(buffer2, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }

                    catch (ObjectDisposedException)
                    {
                        Locker.EnterWriteLock();
                        try
                        {
                            Clients.Remove(client);
                            i--;
                        }
                        finally
                        {
                            Locker.ExitWriteLock();
                        }
                    }
                }

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
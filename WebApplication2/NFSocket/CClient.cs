using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.NFSocket
{
    public static class SendToAll
    {
        public static CSocket sock;
        public static void SendData(string msg, dynamic data){
            sock.send(msg, data);
        }
        public static List<string> getActiveUsers()
        {
            return sock.getActiveUsers();
        }
    }
    public class CSocket
    {
        public List<CClient> clients;
        public object loocker;
        public CSocket(object paramToLock)
        {
            loocker = paramToLock;
            clients = new List<CClient>();
            System.Threading.Thread thread = new System.Threading.Thread(() => checkClientIsAlive(loocker, clients));
            thread.Start();
        }
        public void checkClientIsAlive(object loocker, List<CClient> clients)
        {
            while (true)
            {
                lock (loocker) 
                {
                    try {
                        foreach(var cl in clients)
                        {
                            if(cl.lastUpdate.AddSeconds(40)<DateTime.Now)
                            {
                                var MessageId = Guid.NewGuid().ToString();
                                clients.ForEach(l =>
                                {
                                    l.messagesTo.Add(new CMessageTo()
                                    {
                                        id = MessageId,
                                        msg = "userDisconnect",
                                        data =  new {userId=cl.userId}
                                    });
                                });
                                clients.Remove(cl);
                            }
                        }
                    }
                    catch { }
                }
                System.Threading.Thread.Sleep(1000 * 20);
            }
        }
        public string clientPing(string clientId)
        {
            var ret="";
            lock(loocker)
            {
                var cl = clients.Where(c => c.id == clientId);
                if(cl.Count()<1)
                {
                    
                    clients.Add(new CClient() { id = clientId });
                }
               while(clients.Where(c => c.id == clientId).Count()>1)
               {
                   clients.Remove(clients.Where(c => c.id == clientId).First());
               }

               var client = clients.Where(c => c.id == clientId).First();
               client.lastUpdate = DateTime.Now;
               ret = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(client.messagesTo);
            }
            return ret;
        }
        public void send(string msg, dynamic data)
        {
            lock (loocker)
            {
                var MessageId=Guid.NewGuid().ToString();
                clients.ForEach(l => {
                    l.messagesTo.Add(new CMessageTo()
                    {
                        id = MessageId,
                        msg = msg,
                        data = data
                    });
                });
            }

        }
        public List<string> getActiveUsers()
        {
            var ret = new List<string>();
            lock (loocker)
            {
                clients.ForEach(cl => {
                    ret.Add(cl.userId);
                });
            }
            return ret;
        }
        public void commandComplite(string clientId, string commandId)
        {
            lock (loocker)
            {
                clients.Where(c => c.id == clientId).ToList().ForEach(l => { 
                    
                   l.messagesTo.RemoveAll(m=>m.id==commandId); 
                
                });
                
            }
        }
        public void clientAdd(string id, string userId, string userName)
        {
            lock(loocker)
            {
                clients.ForEach(l => { 
                    var mesid=Guid.NewGuid().ToString();
                    l.messagesTo.Add(new CMessageTo()
                    {
                        id = mesid,
                        msg = "userConnect",
                        data = new { userId = userId, userName = userName }
                    });
                });
                clients.Add(new CClient() {
                id=id,
                 userId=userId,
                 userName=userName
                });
            }
        }
    }
    public  class CClient
    {
        public  string id;
        public  List<CMessageTo> messagesTo;
        public  List<CMessageFrom> messagesFrom;
        public  DateTime lastUpdate;
        public string userId;
        public string userName;

        public  CClient()
        {
            id = Guid.NewGuid().ToString();
            messagesTo = new List<CMessageTo>();
            messagesFrom = new List<CMessageFrom>();
            lastUpdate = DateTime.Now;
        }
    }
    public  class CMessageTo
    {
        public  string id;
        public  string msg;
        public  dynamic data;
    }
    public  class CMessageFrom
    {
        public static string id;
        public static string msg;
        public static dynamic data;
    }
}
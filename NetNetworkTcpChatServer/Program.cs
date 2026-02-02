// TCP CHAT LISTENER

using NetNetworkTcpChatServer;
using System.Net;
using System.Net.Sockets;

TcpChatListener server = new TcpChatListener();
await server.ListenAsync();
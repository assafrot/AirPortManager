
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Server.Interfaces;

namespace Server.Interfaces
{
  public class WebSocketRequestHandler : IWebSocketRequestHandler
  {

    public WebSocketRequestHandler( IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
      sockets = new List<WebSocket>();
    }

    List<WebSocket> sockets;
    IServiceProvider _serviceProvider;

    IWebSocketMessenger CreateSocketMessenger(WebSocket socket) {
      var wsm = new WebSocketMessenger();
      wsm.Socket = socket;
      return wsm;
    }

    public void AddSocket(WebSocket socket)
    {
      sockets.Add(socket);
      var wsm = CreateSocketMessenger(socket);
      wsm.Send("message", "hello from server");
    }

  }

}
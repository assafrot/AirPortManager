using System;
using System.Net.WebSockets;

namespace Server.Interfaces
{
  public interface IWebSocketMessenger
  {
    WebSocket Socket { get; set; }
    event Action OnClose;
    void On(string action, Action callback);
    void On<T>(string action, Action<T> callback);
    void Send<T>(string action, T obj);
  }
}
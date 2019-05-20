
using System.Net.WebSockets;
namespace Server.Interfaces
{
  public interface IWebSocketRequestHandler
  {
    void AddSocket(WebSocket socket);
  }
}
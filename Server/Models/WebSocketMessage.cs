
namespace Server.Models
{

  public class WebSocketMessage
  {
    public string Action { get; set; }
    public object Payload { get; set; }
  }

}
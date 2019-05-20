using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Server.Extenstions;
using Server.Interfaces;
using Server.Models;

namespace Server.Interfaces
{
  public class WebSocketMessenger : IWebSocketMessenger
  {
    public WebSocketMessenger() 
    {
      callbacks = new Dictionary<string, Action<object>>();
    }
    Dictionary<string, Action<object>> callbacks;
    WebSocket _socket;
    public WebSocket Socket
    {
      get => _socket;
      set
      {
        _socket = value;
        InitSocket();
      }
    }

    public event Action OnClose;

    async void InitSocket()
    {
      var buffer = new byte[1024 * 4];
      
      WebSocketReceiveResult result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
      
      while (!result.CloseStatus.HasValue)
      {
        OnMessage(buffer.GetString(result.Count));
        result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
      }

      await _socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
      OnClose?.Invoke();
    }

    void OnMessage(string json) 
    {
      var message = (WebSocketMessage)JsonConvert.DeserializeObject(json);
      if(callbacks.Keys.Contains(message.Action))
      {
        callbacks[message.Action]?.Invoke(message.Payload);
      }
    }

    public void On<T>(string action, Action<T> callback)
    {
      callbacks[action] += (playload) => callback((T)playload);
    }

    public void On(string action, Action callback)
    {
      callbacks[action] += (playload) => callback();
    }

    public async void Send<T>(string action, T obj)
    {
      var message = JsonConvert.SerializeObject(new WebSocketMessage() { Action = action, Payload = obj });
      var byteArray = Encoding.UTF8.GetBytes(message);
      await _socket.SendAsync(new ArraySegment<byte>(byteArray, 0, byteArray.Length), WebSocketMessageType.Text, true, CancellationToken.None);
    }

  }

}

using GoogleDocs.WebSockets.Handler;
using System.Net.WebSockets;

namespace GoogleDocs.WebSockets.Middlewares;

public class WebSocketMiddleware
{
    private readonly WebSocketHandler _webSocketHandler;
    private readonly RequestDelegate _nextDelegate;

    public WebSocketMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
    {
        _webSocketHandler = webSocketHandler;
        _nextDelegate = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (string.IsNullOrEmpty(context.Request.Query["identificador"]))
        {
            throw new ArgumentNullException("Identificador do usuário não informado", "identificador");
        }

        string identificador = context.Request.Query["identificador"]!;

        if (context.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            _webSocketHandler.AdicioneCliente(identificador, webSocket);
            await _webSocketHandler.ProcesseMensagens(identificador, webSocket);
            await _nextDelegate.Invoke(context);
        }

        await _nextDelegate.Invoke(context);
    }
}

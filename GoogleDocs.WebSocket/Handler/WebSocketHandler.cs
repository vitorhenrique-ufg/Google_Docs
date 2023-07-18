using GoogleDocs.Aplicacao.Comandos;
using GoogleDocs.Aplicacao.Requisicoes;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace GoogleDocs.WebSockets.Handler;

public class WebSocketHandler
{
    private readonly ConcurrentDictionary<string, WebSocket> _clientes = new();
    private readonly IComandoInsiraDocumentoTexto _comandoInsiraDocumentoTexto;

    public WebSocketHandler(IComandoInsiraDocumentoTexto comandoInsiraDocumentoTexto)
    {
        _comandoInsiraDocumentoTexto = comandoInsiraDocumentoTexto;

        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(10_000);

                var conexoesFechadas = _clientes.Where(c => c.Value.State != WebSocketState.Open);

                foreach (KeyValuePair<string, WebSocket> item in conexoesFechadas)
                {
                    _clientes.TryRemove(item.Key, out WebSocket _);
                }
            }
        });
    }

    public void AdicioneCliente(string identificador, WebSocket webSocket)
    {
        _clientes.TryAdd(identificador, webSocket);
    }

    public async Task ProcesseMensagens(string identificador, WebSocket webSocket)
    {
        while (Equals(webSocket.State, WebSocketState.Open))
        {
            byte[] buffer = new byte[1024];
            WebSocketReceiveResult resultado = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (resultado is null || !EhResultadoValido(resultado, webSocket))
            {
                continue;
            }

            if (buffer.Length > resultado.Count)
            {
                Array.Resize(ref buffer, resultado.Count);
            }

            await InsiraMensagem(buffer);

            IEnumerable<Task> tarefas = _clientes.Where(socket => Equals(socket.Value.State, WebSocketState.Open) && socket.Key != identificador)
                                                 .Select(cliente => EnvieMensagem(cliente.Value, buffer, resultado));
            await Task.WhenAll(tarefas);
        }
    }

    private static async Task EnvieMensagem(WebSocket cliente, byte[] buffer, WebSocketReceiveResult resultado) =>
        await cliente.SendAsync(new ArraySegment<byte>(
            buffer, 0, resultado.Count), resultado.MessageType, resultado.EndOfMessage, CancellationToken.None);

    private async Task InsiraMensagem(byte[] buffer)
    {
        string textoDocumento = Encoding.UTF8.GetString(buffer);
        DocumentoRequisicao requisicao = JsonSerializer.Deserialize<DocumentoRequisicao>(textoDocumento)
            ?? throw new Exception("Ocorreu um erro ao obter o texto do documento");
        await _comandoInsiraDocumentoTexto.Execute(requisicao);
    }

    private static bool EhResultadoValido(WebSocketReceiveResult resultado, WebSocket webSocket)
    {
        if (Equals(resultado.MessageType, WebSocketMessageType.Close))
        {
            webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Conexão fechada", CancellationToken.None);
            return false;
        }

        return resultado.Count > 0;
    }
}


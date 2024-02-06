using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketTest.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("enter to continiue");
            Console.ReadLine();

            using (var client = new ClientWebSocket())
            {
                var url = new Uri("ws://localhost:5096/send");
                var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(1200));
                try
                {
                    await client.ConnectAsync(url, cts.Token);

                    while (client.State == WebSocketState.Open)
                    {
                        await Console.Out.WriteLineAsync("Введите сообщение для отправки");
                        string message = Console.ReadLine();

                        if (!string.IsNullOrEmpty(message))
                        {
                            var byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                            await client.SendAsync(byteToSend, WebSocketMessageType.Text, false, cts.Token);

                            var responseBuffer = new byte[1024];
                            var offset = 0;
                            var packet = 1024;

                            var byteReceived = new ArraySegment<byte>(responseBuffer, offset, packet);
                            var response = await client.ReceiveAsync(byteReceived, cts.Token);

                            if (response.MessageType == WebSocketMessageType.Text)
                            {
                                string responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                                await Console.Out.WriteLineAsync(responseMessage);
                            }

                            if (response.MessageType == WebSocketMessageType.Close)
                            {
                                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cts.Token);
                                break;
                            }
                        }
                    }

                    Console.ReadLine();
                }
                catch (WebSocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}

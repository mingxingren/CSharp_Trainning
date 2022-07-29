using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketListener {
    // 该方法实际上是同步执行
    public static async Task StartSever() {
        // 异步 获取ip地址
        IPHostEntry host =  await Dns.GetHostEntryAsync("localhost");
        IPAddress iPAddress_V6 = host.AddressList[0];
        IPAddress iPAddress_V4 = host.AddressList[1];

        IPEndPoint localEndPoint = new IPEndPoint(iPAddress_V4, 9999);

        try{
            Socket listener = new Socket(iPAddress_V4.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);
            Console.WriteLine("start listen server, host: {0}", localEndPoint);
            Socket handler = await listener.AcceptAsync();
            Console.WriteLine("accept a client ipaddress: {0}", handler.LocalEndPoint);
            string? data = null;
            byte[]? bytes = null;

            while (true) {
                bytes = new byte[1024];
                int bytesRec = await handler.ReceiveAsync(new Memory<byte>(bytes), SocketFlags.None);
                data += Encoding.ASCII.GetString(bytes, 0 , bytesRec);
                // 判断链接断开
                if (data.IndexOf("<EOF>") > -1) {
                    break;
                }
            }

            byte[] msg = Encoding.ASCII.GetBytes(data);
            // 发送
            await handler.SendAsync(new ArraySegment<byte>(msg), SocketFlags.None);
            // 收通道 和 发通道全部关闭
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            Console.WriteLine("Close server");
        }
        catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }
}
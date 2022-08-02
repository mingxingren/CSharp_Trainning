using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketListener {
    private Socket? m_listener;
    private IPEndPoint? m_localEndPoint;

    // 初始化函数
    public async Task<bool> Init(int port) {
        try {
            IPHostEntry host =  await Dns.GetHostEntryAsync("localhost");
            IPAddress iPAddress_V6 = host.AddressList[0];
            IPAddress iPAddress_V4 = host.AddressList[1];

            m_localEndPoint = new IPEndPoint(iPAddress_V4, 9999);

            m_listener = new Socket(iPAddress_V4.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_listener.Bind(m_localEndPoint);
            m_listener.Listen(10);
            return true;
        }
        catch (Exception e) {
            Console.WriteLine(e.ToString());
            return false;
        }
    }

    public async Task<Socket> Listen() {
        Console.WriteLine("start listen server, host: {0}", m_localEndPoint);
        Socket handler = await m_listener!.AcceptAsync();
        Console.WriteLine("accept a client ipaddress: {0}", handler.LocalEndPoint);
        return handler;
    }

    // 该方法实际上是同步执行
    public async Task Communication(Socket handler) {        
        try{
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
using System.Threading.Tasks;
using System.Net.Sockets;


// See https://aka.ms/new-console-template for more information
namespace Program {
    class App {    
        static void Main(string[] args) {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync() {
            SocketListener server = new SocketListener();
            bool res = await server.Init(9999);
            if (!res) {
                return;
            }

            while (true) {
                Socket handler = await server.Listen();
                _ = server.Communication(handler);
            }
        }
    }
}

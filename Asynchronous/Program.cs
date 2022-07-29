// See https://aka.ms/new-console-template for more information
namespace Program {
    class App {    
        static async Task Main(string[] args) {
            await SocketListener.StartSever();
        }
    }
}

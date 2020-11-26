using ConsoleApp;
using System.Threading.Tasks;

namespace Core.Plugins.Samples
{
    class Program : ConsoleAppProgram<Startup>
    {
        static async Task Main(string[] args)
        {
            await RunAsync();
        }
    }
}

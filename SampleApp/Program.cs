using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            ConsoleCtrlHandler.StartMonitor(cancellationTokenSource);

            try
            {
                string str = "51.347846, -3.17562";

                var (success, lat, lng) = str.TryParseLatitudeLongitude();

                if (success)
                {
                    Console.WriteLine($"Coordinates={lat},{lng}");
                }

                Console.WriteLine("Starting task");

                await Task.Delay(10000, cancellationTokenSource.Token);

                Console.WriteLine("Finished task");
            }
            catch (TaskCanceledException)
            {
                // ignore
            }
            if (cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("Cancellation requested");
                //break;
            }

            Console.WriteLine("Press ENTER to quit");
            _ = Console.ReadLine();
        }
    }
}

// Version: 20200914.1112
// Auto-generated file. DO NOT MODIFY.

using System;
using System.Threading;

namespace SampleApp
{
    internal static partial class ConsoleCtrlHandler
    {
        private static CancellationTokenSource _cancellationTokenSource;

        private static void ConsoleCtrlCheck(object sender, ConsoleCancelEventArgs args)
        {
            _cancellationTokenSource.Cancel();

            args.Cancel = true; // Don't exit immediately
        }

        public static void StartMonitor(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;

            Console.CancelKeyPress += new ConsoleCancelEventHandler(ConsoleCtrlCheck);
        }
    }
}

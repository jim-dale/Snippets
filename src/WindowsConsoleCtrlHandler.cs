using System.Runtime.InteropServices;
using System.Threading;

namespace __Snippets__
{
    internal static partial class WindowsConsoleCtrlHandler
    {
        private delegate bool HandlerRoutine(CtrlType sig);

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);

        private static CancellationTokenSource _cancellationTokenSource;

        private static bool ConsoleCtrlCheck(CtrlType sig)
        {
            _cancellationTokenSource.Cancel();
            return true;
        }

        public static void StartMonitor(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;

            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
        }
    }
}

using System.Runtime.ExceptionServices;

namespace Main.Support_Tools
{
    public class Debugger
    {
        public static bool Enabled { get; set; } = false; //Default is false for production builds
        public static void Handle(Exception exCaught, Action failsafeMethod = null)
        {
            // Always log the exception
            Console.WriteLine($"DEBUG: Exception caught: {exCaught}");

            if (Enabled)
            {
                // Re-throw for debugger to catch
                ExceptionDispatchInfo.Capture(exCaught).Throw();
            }
            else
            {
                // Call the optional failsafe method if provided
                failsafeMethod?.Invoke();
            }
        }
        public static void AttachGlobalHandlers(Action failsafeMethod = null)
        {
            //Last-resort alternative to try-catch

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    Handle(ex, failsafeMethod);
                }
            }; //For unhandled exceptions coming from background taks

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Handle(args.Exception, failsafeMethod);
                args.SetObserved(); // prevents process termination in some cases
            }; //For async/await tasks

            Application.ThreadException += (sender, args) =>
            {
                Handle(args.Exception, failsafeMethod);
            };
        }
    }
}

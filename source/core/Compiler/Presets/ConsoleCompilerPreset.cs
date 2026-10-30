using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SHVDN.Compiler.Presets
{
    /// <summary>
    /// The <see cref="CompilerPreset"/> for compiling console expressions.
    /// </summary>
    public class ConsoleCompilerPreset : CompilerPreset
    {
        private static ConsoleCompilerPreset s_instance;

        private static int s_initializationStarted;

        private ConsoleCompilerPreset()
        {
            AddReference(typeof(object).Assembly.Location);
            AddReference("System.dll");
            AddReference("System.Core.dll");
            AddReference("System.Drawing.dll");
            AddReference("System.Windows.Forms.dll");

            // Reference the newest scripting API
            AddReference("ScriptHookVDotNet3.dll");
            AddReference(typeof(ScriptHookVDotNet).Assembly.Location);

            // With `allowUnsafe`, you can use natives that require accessible addresses without having to use
            // members of the Marshall class (e.g. SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED)
            Options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true,
                optimizationLevel: OptimizationLevel.Debug);
        }

        /// <remarks>
        /// Returns <see langword="null"/> until <see cref="TryCreateOnNewThread"/> has been called.
        /// </remarks>
        public static ConsoleCompilerPreset Instance
        {
            get => Volatile.Read(ref s_instance);
            private set => Volatile.Write(ref s_instance, value);
        }

        public static void TryCreateOnNewThread()
        {
            if (Instance != null || Interlocked.CompareExchange(ref s_initializationStarted, 1, 0) != 0)
            {
                return;
            }

            var thread = new Thread(() =>
            {
                Instance = new ConsoleCompilerPreset();
            })
            {
                IsBackground = true
            };

            thread.Start();
        }
    }
}

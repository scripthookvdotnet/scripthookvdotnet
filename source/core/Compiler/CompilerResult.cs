using System.Reflection;

namespace SHVDN.Compiler
{
    public readonly struct CompilerResult
    {
        /// <summary>
        /// Initialised a new instance of <see cref="CompilerResult"/>.
        /// </summary>
        /// <param name="compiledAssembly">The compiled assembly.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorCount">The total count of errors.</param>
        public CompilerResult(Assembly compiledAssembly, string errorMessage, int errorCount)
        {
            CompiledAssembly = compiledAssembly;
            ErrorMessage = errorMessage;
            ErrorCount = errorCount;
        }

        /// <summary>
        /// Gets the compiled assembly.
        /// </summary>
        public Assembly CompiledAssembly { get; }

        /// <summary>
        /// Gets the fully assembled error message for any errors in this <see cref="CompilerResult"/>.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Gets the count of how many errors occured during compilation.
        /// </summary>
        public int ErrorCount { get; }


        /// <summary>
        /// Determines whether this <see cref="CompilerResult"/> contains any errors.
        /// </summary>
        public bool HasErrors => CompiledAssembly == null;
    }
}

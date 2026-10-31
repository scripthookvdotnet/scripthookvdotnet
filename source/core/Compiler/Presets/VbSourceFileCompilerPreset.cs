using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SHVDN.Compiler.Presets
{
    /// <summary>
    /// The <see cref="CompilerPreset"/> for compiling source files in Visual Basic.
    /// </summary>
    public class VbSourceFileCompilerPreset : CompilerPreset
    {
        private static VbSourceFileCompilerPreset _instance;

        private VbSourceFileCompilerPreset()
        {
            AddReference(typeof(object).Assembly.Location);
            AddReference("System.dll");
            AddReference("System.Core.dll");
            AddReference("System.Drawing.dll");
            AddReference("System.Windows.Forms.dll");
            AddReference("System.XML.dll");
            AddReference("System.XML.Linq.dll");
            AddReference(typeof(ScriptHookVDotNet).Assembly.Location);

            Options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
        }

        public static VbSourceFileCompilerPreset Instance
        {
            get => _instance ??= new VbSourceFileCompilerPreset();
        }
    }
}

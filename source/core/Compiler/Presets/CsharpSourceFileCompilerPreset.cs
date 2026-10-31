using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SHVDN.Compiler.Presets
{
    /// <summary>
    /// The <see cref="CompilerPreset"/> for compiling source files in CSharp.
    /// </summary>
    public class CsharpSourceFileCompilerPreset : CompilerPreset
    {
        private static CsharpSourceFileCompilerPreset _instance;

        private CsharpSourceFileCompilerPreset()
        {
            AddReference(typeof(object).Assembly.Location);
            AddReference("System.dll");
            AddReference("System.Core.dll");
            AddReference("System.Drawing.dll");
            AddReference("System.Windows.Forms.dll");
            AddReference("System.XML.dll");
            AddReference("System.XML.Linq.dll");
            AddReference(typeof(ScriptHookVDotNet).Assembly.Location);

            // With `allowUnsafe`, you can use natives that require accessible addresses without having to use
            // members of the Marshall class (e.g. SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED)
            Options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);
        }

        public static CsharpSourceFileCompilerPreset Instance
        {
            get => _instance ??= new CsharpSourceFileCompilerPreset();
        }
    }
}

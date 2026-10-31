using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.VisualBasic;
using LanguageVersion = Microsoft.CodeAnalysis.VisualBasic.LanguageVersion;

namespace SHVDN.Compiler
{
    public static class CodeCompiler
    {
        /// <summary>
        /// Compiles and loads the give source code(either CSharp, or Visual Basic) with the given <see cref="CompilerPreset"/> into an assembly.
        /// </summary>
        /// <param name="preset">The preset to compile against.</param>
        /// <param name="source">The source to compile.</param>
        /// <returns>The result of the compilation.</returns>
        public static CompilerResult CompileToAssembly(CompilerPreset preset, string source)
        {
            Compilation compilation = Compile(preset, source);

            return CreateAssembly(compilation);
        }

        /// <summary>
        /// Compiles the given source code(either CSharp, or Visual Basic) with the given <see cref="CompilerPreset"/>.
        /// </summary>
        /// <param name="preset">The preset to compile against.</param>
        /// <param name="source">The source to compile</param>
        /// <returns>
        /// <see cref="Compilation"/> if the source was compiled without any errors; otherwise <see langword="null"/>.
        /// </returns>
        public static Compilation Compile(CompilerPreset preset, string source)
        {
            if (preset == null)
            {
                return null;
            }

            List<MetadataReference> references = preset.References.ToList();

            foreach (Script script in ScriptDomain.CurrentDomain.RunningScripts.Where(x => x.IsRunning))
            {
                if (File.Exists(script.Filename) && Path.GetExtension(script.Filename) == ".dll")
                {
                    CompilerPreset.AddReferenceToMetadata(references, script.Filename);
                }
            }

            if (preset.Options is CSharpCompilationOptions csharpCompileOptions)
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(Microsoft.CodeAnalysis.CSharp.LanguageVersion.CSharp14));

                return CSharpCompilation.Create(
                    assemblyName: Guid.NewGuid().ToString("N"),
                    syntaxTrees: new[] { syntaxTree },
                    references: references,
                    options: csharpCompileOptions);
            }

            if (preset.Options is VisualBasicCompilationOptions vbCompileOptions)
            {
                SyntaxTree syntaxTree = VisualBasicSyntaxTree.ParseText(source, new VisualBasicParseOptions(LanguageVersion.VisualBasic17_13));

                return VisualBasicCompilation.Create(
                    assemblyName: Guid.NewGuid().ToString("N"),
                    syntaxTrees: new[] { syntaxTree },
                    references: references,
                    options: vbCompileOptions);
            }

            return null;
        }

        /// <summary>
        /// Creates an <see cref="Assembly"/> from the given compilation.
        /// </summary>
        /// <param name="compilation">The compilation to load into an <see cref="Assembly"/>.</param>
        /// <returns>The result of the compilation.</returns>
        public static CompilerResult CreateAssembly(Compilation compilation)
        {
            using MemoryStream stream = new();

            EmitResult result = compilation.Emit(stream);

            if (result.Success)
            {
                return new CompilerResult(Assembly.Load(stream.ToArray()), string.Empty, 0);
            }

            string errors = CollectErrorsFromEmit(result, out int errorCount);

            return new CompilerResult(null, errors, errorCount);
        }

        private static string CollectErrorsFromEmit(EmitResult result, out int errorCount)
        {
            Diagnostic[] diagnostics = result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();

            var errors = new StringBuilder();

            for (int i = 0; i < diagnostics.Length; i++)
            {
                Diagnostic diagnostic = diagnostics[i];
                FileLinePositionSpan span = diagnostic.Location.GetLineSpan();

                errors.Append("   at line ");
                errors.Append(span.StartLinePosition.Line + 1);
                errors.Append(": ");
                errors.Append(diagnostic.GetMessage());

                if (i < diagnostics.Length - 1)
                {
                    errors.AppendLine();
                }
            }

            errorCount = diagnostics.Length;
            return errors.ToString();
        }
    }
}

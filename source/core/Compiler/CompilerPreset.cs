using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;

namespace SHVDN.Compiler
{
    public abstract class CompilerPreset
    {
        private static readonly string[] s_knownReferenceDirectories =
        {
            Path.GetDirectoryName(typeof(Object).Assembly.Location),
            Path.GetDirectoryName(typeof(ScriptHookVDotNet).Assembly.Location),
        };

        /// <summary>
        /// Gets the list of references of this <see cref="CompilerPreset"/>.
        /// </summary>
        public List<MetadataReference> References { get; } = new();

        /// <summary>
        /// Gets the <see cref="CompilationOptions"/> of this <see cref="CompilerPreset"/>.
        /// </summary>
        public CompilationOptions Options { get; protected set; }

        /// <summary>
        /// Resolves and adds the given reference to this <see cref="CompilerPreset"/>.
        /// </summary>
        /// <param name="path">The name or the path of the reference.</param>
        /// <returns>
        /// <see langword="true"/> if the reference could be resolved; otherwise <see langword="false"/>.
        /// </returns>
        protected bool AddReference(string path)
        {
            return AddReferenceToMetadata(References, path);
        }

        /// <summary>
        /// Resolves and adds the given reference to the given reference list.
        /// </summary>
        /// <param name="references">The list to add the reference to.</param>
        /// <param name="path">The name or the path of the reference.</param>
        /// <returns>
        /// <see langword="true"/> if the reference could be resolved; otherwise <see langword="false"/>.
        /// </returns>
        public static bool AddReferenceToMetadata(List<MetadataReference> references, string path)
        {
            string absolutePath = GetReferencePath(path);

            if (absolutePath == string.Empty)
            {
                return false;
            }

            references.Add(MetadataReference.CreateFromFile(absolutePath));

            return true;
        }

        private static string GetReferencePath(string path)
        {
            if (File.Exists(path))
            {
                return path;
            }

            foreach (string directory in s_knownReferenceDirectories)
            {
                string candidate = Path.Combine(directory, path);

                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            Log.Message(Log.Level.Error, new Log.Options { ForceLogToConsole = true }, $"Failed to resolve '{path}' assembly path.");

            return string.Empty;
        }
    }
}

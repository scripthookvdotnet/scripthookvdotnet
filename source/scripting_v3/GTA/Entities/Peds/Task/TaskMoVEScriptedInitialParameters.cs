//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Runtime.InteropServices;

namespace GTA
{
    /// <summary>
    /// Represents an immutable set of initial parameters for scripted move network tasks.
    /// </summary>
    public sealed class TaskMoVEScriptedInitialParameters
    {
        internal TaskMoVEScriptedInitialParameters(AtHashValue clipSet0, AtHashValue varclipSet0,
            AtHashValue clipSet1, AtHashValue varclipSet1, string floatParamName0, float floatParamValue0,
            float floatParamLerpValue0, string floatParamName1, float floatParamValue1, float floatParamLerpValue1,
            string boolParamName0, bool boolParamValue0, string boolParamName1, bool boolParamValue1)
        {
            ClipSet0 = clipSet0;
            VarClipSet0 = varclipSet0;
            ClipSet1 = clipSet1;
            VarClipSet1 = varclipSet1;

            FloatParamName0 = floatParamName0;
            FloatParamValue0 = floatParamValue0;
            FloatParamLerpValue0 = floatParamLerpValue0;
            FloatParamName1 = floatParamName1;
            FloatParamValue1 = floatParamValue1;
            FloatParamLerpValue1 = floatParamLerpValue1;

            BoolParamName0 = boolParamName0;
            BoolParamValue0 = boolParamValue0;
            BoolParamName1 = boolParamName1;
            BoolParamValue1 = boolParamValue1;
        }

        public AtHashValue ClipSet0 { get; }

        public AtHashValue VarClipSet0 { get; }

        public AtHashValue ClipSet1 { get; }

        public AtHashValue VarClipSet1 { get; }

        public string FloatParamName0 { get; }

        public float FloatParamValue0 { get; }

        public float FloatParamLerpValue0 { get; }

        public string FloatParamName1 { get; }

        public float FloatParamValue1 { get; }

        public float FloatParamLerpValue1 { get; }

        public string BoolParamName0 { get; }

        public bool BoolParamValue0 { get; }

        public string BoolParamName1 { get; }

        public bool BoolParamValue1 { get; }

        /// <remarks>
        /// Builds a new <see cref="TaskMoVEScriptedInitialParametersStruct"/> instance to pass to native functions.
        /// </remarks>
        internal TaskMoVEScriptedInitialParametersStruct BuildStructForNatives()
        {
            IntPtr floatParamNamePtr0 = FloatParamName0 != null
                ? SHVDN.StringMarshal.StringToCoTaskMemUtf8(FloatParamName0)
                : IntPtr.Zero;
            IntPtr floatParamNamePtr1 = FloatParamName1 != null
                ? SHVDN.StringMarshal.StringToCoTaskMemUtf8(FloatParamName1)
                : IntPtr.Zero;
            IntPtr boolParamNamePtr0 = BoolParamName0 != null
                ? SHVDN.StringMarshal.StringToCoTaskMemUtf8(BoolParamName0)
                : IntPtr.Zero;
            IntPtr boolParamNamePtr1 = BoolParamName1 != null
                ? SHVDN.StringMarshal.StringToCoTaskMemUtf8(BoolParamName1)
                : IntPtr.Zero;

            return new TaskMoVEScriptedInitialParametersStruct(ClipSet0, VarClipSet0, ClipSet1, VarClipSet1,
                               floatParamNamePtr0, FloatParamValue0, FloatParamLerpValue0, floatParamNamePtr1,
                               FloatParamValue1, FloatParamLerpValue1, boolParamNamePtr0, BoolParamValue0,
                               boolParamNamePtr1, BoolParamValue1);
        }
    }

    /// <summary>
    /// Represents a builder that builds <see cref="TaskMoVEScriptedInitialParameters"/>.
    /// </summary>
    public sealed class TaskMoVEScriptedInitialParametersBuilder
    {
        public TaskMoVEScriptedInitialParametersBuilder()
        {
            // These 2 lerp variables should be initialized to -1.0f as how `MOVE_INITIAL_PARAMETERS` in
            // `commands_task.sch` and `CTaskMoVEScripted::InitialParameters` suggest.
            _floatParamLerpValue0 = -1.0f;
            _floatParamLerpValue1 = -1.0f;
        }

        private AtHashValue _clipSet0;

        private AtHashValue _varClipSet0;

        private AtHashValue _clipSet1;

        private AtHashValue _varClipSet1;

        private string _floatParamName0;

        private float _floatParamValue0;

        private float _floatParamLerpValue0;

        private string _floatParamName1;

        private float _floatParamValue1;

        private float _floatParamLerpValue1;

        private string _boolParamName0;

        private bool _boolParamValue0;

        private string _boolParamName1;

        private bool _boolParamValue1;

        public TaskMoVEScriptedInitialParametersBuilder ClipSet0(AtHashValue clipSet0)
        {
            _clipSet0 = clipSet0;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder VarClipSet0(AtHashValue varClipSet0)
        {
            _varClipSet0 = varClipSet0;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder ClipSet1(AtHashValue clipSet1)
        {
            _clipSet1 = clipSet1;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder VarClipSet1(AtHashValue varClipSet1)
        {
            _varClipSet1 = varClipSet1;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder FloatParamName0(string floatParamName0)
        {
            _floatParamName0 = floatParamName0;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder FloatParamValue0(float floatParamValue0)
        {
            _floatParamValue0 = floatParamValue0;
            return this;
        }

        /// <remarks>
        /// The default value is -1.0f.
        /// </remarks>
        public TaskMoVEScriptedInitialParametersBuilder FloatParamLerpValue0(float floatParamLerpValue0)
        {
            _floatParamLerpValue0 = floatParamLerpValue0;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder FloatParamName1(string floatParamName1)
        {
            _floatParamName1 = floatParamName1;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder FloatParamValue1(float floatParamValue1)
        {
            _floatParamValue1 = floatParamValue1;
            return this;
        }

        /// <remarks>
        /// The default value is -1.0f.
        /// </remarks>
        public TaskMoVEScriptedInitialParametersBuilder FloatParamLerpValue1(float floatParamLerpValue1)
        {
            _floatParamLerpValue1 = floatParamLerpValue1;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder BoolParamName0(string boolParamName0)
        {
            _boolParamName0 = boolParamName0;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder BoolParamValue0(bool boolParamValue0)
        {
            _boolParamValue0 = boolParamValue0;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder BoolParamName1(string boolParamName1)
        {
            _boolParamName1 = boolParamName1;
            return this;
        }

        public TaskMoVEScriptedInitialParametersBuilder BoolParamValue1(bool boolParamValue1)
        {
            _boolParamValue1 = boolParamValue1;
            return this;
        }

        /// <remarks>
        /// Builds a new <see cref="TaskMoVEScriptedInitialParameters"/> instance.
        /// </remarks>
        public TaskMoVEScriptedInitialParameters Build()
        {
            return new TaskMoVEScriptedInitialParameters(_clipSet0, _varClipSet0, _clipSet1, _varClipSet1,
                               _floatParamName0, _floatParamValue0, _floatParamLerpValue0, _floatParamName1,
                               _floatParamValue1, _floatParamLerpValue1, _boolParamName0, _boolParamValue0,
                               _boolParamName1, _boolParamValue1);
        }
    }

    /// <summary>
    /// Represents an immutable struct that has a set of initial parameters and for scripted move network tasks.
    /// Intended to be used to pass the data to native functions.
    /// </summary>
    /// <remarks>
    /// Since this struct has a private disposed field, this struct should not be passed by value as an argument.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal ref struct TaskMoVEScriptedInitialParametersStruct
    {
        internal TaskMoVEScriptedInitialParametersStruct(AtHashValue clipSet0, AtHashValue varclipSet0,
            AtHashValue clipSet1, AtHashValue varclipSet1, IntPtr floatParamNamePtr0, float floatParamValue0,
            float floatParamLerpValue0, IntPtr floatParamNamePtr1, float floatParamValue1, float floatParamLerpValue1,
            IntPtr boolParamNamePtr0, bool boolParamValue0, IntPtr boolParamNamePtr1, bool boolParamValue1)
        {
            ClipSet0 = clipSet0;
            VarClipSet0 = varclipSet0;
            ClipSet1 = clipSet1;
            VarClipSet1 = varclipSet1;

            FloatParamNamePtr0 = floatParamNamePtr0;
            FloatParamValue0 = floatParamValue0;
            FloatParamLerpValue0 = floatParamLerpValue0;
            FloatParamNamePtr1 = floatParamNamePtr1;
            FloatParamValue1 = floatParamValue1;
            FloatParamLerpValue1 = floatParamLerpValue1;

            BoolParamNamePtr0 = boolParamNamePtr0;
            BoolParamValue0 = boolParamValue0;
            BoolParamNamePtr1 = boolParamNamePtr1;
            BoolParamValue1 = boolParamValue1;

            _disposed = false;
        }

        internal readonly AtHashValue ClipSet0 { get; }

        internal readonly AtHashValue VarClipSet0 { get; }

        internal readonly AtHashValue ClipSet1 { get; }

        internal readonly AtHashValue VarClipSet1 { get; }

        internal readonly IntPtr FloatParamNamePtr0 { get; }

        internal readonly float FloatParamValue0 { get; }

        internal readonly float FloatParamLerpValue0 { get; }

        internal readonly IntPtr FloatParamNamePtr1 { get; }

        internal readonly float FloatParamValue1 { get; }

        internal readonly float FloatParamLerpValue1 { get; }

        internal readonly IntPtr BoolParamNamePtr0 { get; }

        internal readonly bool BoolParamValue0 { get; }

        internal readonly IntPtr BoolParamNamePtr1 { get; }

        internal readonly bool BoolParamValue1 { get; }

        private bool _disposed;

        internal void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (FloatParamNamePtr0 != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(FloatParamNamePtr0);
            }
            if (FloatParamNamePtr1 != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(FloatParamNamePtr1);
            }
            if (BoolParamNamePtr0 != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(BoolParamNamePtr0);
            }
            if (BoolParamNamePtr1 != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(BoolParamNamePtr1);
            }

            _disposed = true;
        }
    }
}

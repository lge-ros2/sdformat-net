// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// Entry point for parsing SDF files/strings. Wraps sdf::Root.
    /// This is the only class that owns native memory and must be disposed.
    /// </summary>
    public sealed class SdfRoot : IDisposable
    {
        private IntPtr _handle;
        private bool _disposed;

        /// <summary>Create a new empty SDF root.</summary>
        public SdfRoot()
        {
            _handle = NativeMethods.sdf_root_create();
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create SDF root.");
        }

        /// <summary>
        /// Load an SDF file from disk.
        /// </summary>
        /// <param name="filePath">Path to the .sdf or .world file.</param>
        /// <exception cref="SdfException">Thrown when parsing errors occur.</exception>
        public void LoadFile(string filePath)
        {
            ThrowIfDisposed();
            int errCount = NativeMethods.sdf_root_load_file(_handle, filePath, out IntPtr errPtr);
            if (errCount > 0)
            {
                string msg = NativeStringHelper.ConsumeStringOrEmpty(errPtr);
                throw new SdfException($"SDF load errors ({errCount}):\n{msg}");
            }
        }

        /// <summary>
        /// Load SDF from a string.
        /// </summary>
        /// <param name="sdfString">An SDF XML string.</param>
        /// <exception cref="SdfException">Thrown when parsing errors occur.</exception>
        public void LoadString(string sdfString)
        {
            ThrowIfDisposed();
            int errCount = NativeMethods.sdf_root_load_string(_handle, sdfString, out IntPtr errPtr);
            if (errCount > 0)
            {
                string msg = NativeStringHelper.ConsumeStringOrEmpty(errPtr);
                throw new SdfException($"SDF load errors ({errCount}):\n{msg}");
            }
        }

        /// <summary>SDF version string.</summary>
        public string Version
        {
            get
            {
                ThrowIfDisposed();
                return NativeStringHelper.ConsumeStringOrEmpty(
                    NativeMethods.sdf_root_version(_handle));
            }
        }

        /// <summary>Number of worlds in this SDF.</summary>
        public ulong WorldCount
        {
            get
            {
                ThrowIfDisposed();
                return NativeMethods.sdf_root_world_count(_handle);
            }
        }

        /// <summary>Get a world by index.</summary>
        public SdfWorld? WorldByIndex(ulong index)
        {
            ThrowIfDisposed();
            IntPtr ptr = NativeMethods.sdf_root_world_by_index(_handle, index);
            return ptr == IntPtr.Zero ? null : new SdfWorld(ptr);
        }

        /// <summary>Get a world by name.</summary>
        public SdfWorld? WorldByName(string name)
        {
            ThrowIfDisposed();
            IntPtr ptr = NativeMethods.sdf_root_world_by_name(_handle, name);
            return ptr == IntPtr.Zero ? null : new SdfWorld(ptr);
        }

        /// <summary>Get the top-level model (if present, for model-only SDF files).</summary>
        public SdfModel? Model
        {
            get
            {
                ThrowIfDisposed();
                IntPtr ptr = NativeMethods.sdf_root_model(_handle);
                return ptr == IntPtr.Zero ? null : new SdfModel(ptr);
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SdfRoot));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                if (_handle != IntPtr.Zero)
                {
                    NativeMethods.sdf_root_destroy(_handle);
                    _handle = IntPtr.Zero;
                }
            }
        }

        ~SdfRoot()
        {
            Dispose();
        }
    }

    /// <summary>
    /// Exception for SDF parsing errors.
    /// </summary>
    public class SdfException : Exception
    {
        public SdfException(string message) : base(message) { }
    }
}

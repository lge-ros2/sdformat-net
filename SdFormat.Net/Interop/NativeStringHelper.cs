// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using System.Runtime.InteropServices;

namespace SdFormat.Interop
{
    /// <summary>
    /// Helpers for marshalling native strings allocated by sdformat_wrapper.
    /// </summary>
    internal static class NativeStringHelper
    {
        /// <summary>
        /// Reads a native UTF-8 string and frees the native buffer.
        /// Returns null if the pointer is IntPtr.Zero.
        /// </summary>
        internal static string? ConsumeString(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;
            try
            {
                return Marshal.PtrToStringUTF8(ptr);
            }
            finally
            {
                NativeMethods.sdf_free_string(ptr);
            }
        }

        /// <summary>
        /// Reads a native UTF-8 string and frees the native buffer.
        /// Returns empty string if the pointer is IntPtr.Zero.
        /// </summary>
        internal static string ConsumeStringOrEmpty(IntPtr ptr)
        {
            return ConsumeString(ptr) ?? string.Empty;
        }
    }
}

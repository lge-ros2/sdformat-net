// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF explicit frame. Wraps sdf::Frame.
    /// </summary>
    public sealed class SdfFrame
    {
        private readonly IntPtr _ptr;

        internal SdfFrame(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Name of the frame.</summary>
        public string Name =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_frame_name(_ptr));

        /// <summary>The body this frame is attached to.</summary>
        public string AttachedTo =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_frame_attached_to(_ptr));

        /// <summary>Raw pose of the frame.</summary>
        public SdfPose3d RawPose
        {
            get
            {
                NativeMethods.sdf_frame_raw_pose(_ptr,
                    out double px, out double py, out double pz,
                    out double rx, out double ry, out double rz, out double rw);
                return new SdfPose3d(px, py, pz, rx, ry, rz, rw);
            }
        }

        /// <summary>Frame this pose is relative to.</summary>
        public string PoseRelativeTo =>
            NativeStringHelper.ConsumeStringOrEmpty(
                NativeMethods.sdf_frame_pose_relative_to(_ptr));

        public override string ToString() => $"Frame(\"{Name}\")";
    }
}

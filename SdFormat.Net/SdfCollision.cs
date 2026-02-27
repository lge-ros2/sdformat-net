// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF collision element. Wraps sdf::Collision.
    /// </summary>
    public sealed class SdfCollision
    {
        private readonly IntPtr _ptr;

        internal SdfCollision(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Name of the collision.</summary>
        public string Name =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_collision_name(_ptr));

        /// <summary>Raw pose of the collision.</summary>
        public SdfPose3d RawPose
        {
            get
            {
                NativeMethods.sdf_collision_raw_pose(_ptr,
                    out double px, out double py, out double pz,
                    out double rx, out double ry, out double rz, out double rw);
                return new SdfPose3d(px, py, pz, rx, ry, rz, rw);
            }
        }

        /// <summary>Geometry of this collision.</summary>
        public SdfGeometry? Geometry
        {
            get
            {
                IntPtr ptr = NativeMethods.sdf_collision_geometry(_ptr);
                return ptr == IntPtr.Zero ? null : new SdfGeometry(ptr);
            }
        }

        public override string ToString() => $"Collision(\"{Name}\")";
    }
}

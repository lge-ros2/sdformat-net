// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF visual element. Wraps sdf::Visual.
    /// </summary>
    public sealed class SdfVisual
    {
        private readonly IntPtr _ptr;

        internal SdfVisual(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Name of the visual.</summary>
        public string Name =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_visual_name(_ptr));

        /// <summary>Whether this visual casts shadows.</summary>
        public bool CastShadows => NativeMethods.sdf_visual_cast_shadows(_ptr) != 0;

        /// <summary>Transparency value (0 = opaque, 1 = fully transparent).</summary>
        public float Transparency => NativeMethods.sdf_visual_transparency(_ptr);

        /// <summary>Raw pose of the visual.</summary>
        public SdfPose3d RawPose
        {
            get
            {
                NativeMethods.sdf_visual_raw_pose(_ptr,
                    out double px, out double py, out double pz,
                    out double rx, out double ry, out double rz, out double rw);
                return new SdfPose3d(px, py, pz, rx, ry, rz, rw);
            }
        }

        /// <summary>The frame this visual's pose is relative to.</summary>
        public string PoseRelativeTo =>
            NativeStringHelper.ConsumeStringOrEmpty(
                NativeMethods.sdf_visual_pose_relative_to(_ptr));

        /// <summary>Geometry of this visual.</summary>
        public SdfGeometry? Geometry
        {
            get
            {
                IntPtr ptr = NativeMethods.sdf_visual_geometry(_ptr);
                return ptr == IntPtr.Zero ? null : new SdfGeometry(ptr);
            }
        }

        /// <summary>Material of this visual.</summary>
        public SdfMaterial? Material
        {
            get
            {
                IntPtr ptr = NativeMethods.sdf_visual_material(_ptr);
                return ptr == IntPtr.Zero ? null : new SdfMaterial(ptr);
            }
        }

        /// <summary>Visibility flags bitmask.</summary>
        public uint VisibilityFlags => NativeMethods.sdf_visual_visibility_flags(_ptr);

        /// <summary>Whether a laser retro value has been set.</summary>
        public bool HasLaserRetro => NativeMethods.sdf_visual_has_laser_retro(_ptr) != 0;

        /// <summary>Laser retro value.</summary>
        public double LaserRetro => NativeMethods.sdf_visual_laser_retro(_ptr);

        public override string ToString() => $"Visual(\"{Name}\")";
    }
}

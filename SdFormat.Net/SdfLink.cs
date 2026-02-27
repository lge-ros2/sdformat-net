// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF link. Wraps sdf::Link.
    /// Non-owning wrapper — lifetime is managed by the parent model.
    /// </summary>
    public sealed class SdfLink
    {
        private readonly IntPtr _ptr;

        internal SdfLink(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Name of the link.</summary>
        public string Name =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_link_name(_ptr));

        /// <summary>Raw pose of the link.</summary>
        public SdfPose3d RawPose
        {
            get
            {
                NativeMethods.sdf_link_raw_pose(_ptr,
                    out double px, out double py, out double pz,
                    out double rx, out double ry, out double rz, out double rw);
                return new SdfPose3d(px, py, pz, rx, ry, rz, rw);
            }
        }

        /// <summary>Frame this link's pose is relative to.</summary>
        public string PoseRelativeTo =>
            NativeStringHelper.ConsumeStringOrEmpty(
                NativeMethods.sdf_link_pose_relative_to(_ptr));

        /// <summary>Whether wind effects are enabled.</summary>
        public bool EnableWind => NativeMethods.sdf_link_enable_wind(_ptr) != 0;

        /// <summary>Whether gravity is enabled.</summary>
        public bool EnableGravity => NativeMethods.sdf_link_enable_gravity(_ptr) != 0;

        /// <summary>Whether the link is kinematic.</summary>
        public bool Kinematic => NativeMethods.sdf_link_kinematic(_ptr) != 0;

        /// <summary>Inertial properties.</summary>
        public SdfInertial Inertial
        {
            get
            {
                NativeMethods.sdf_link_inertial(_ptr,
                    out double mass,
                    out double ixx, out double iyy, out double izz,
                    out double ixy, out double ixz, out double iyz,
                    out double px, out double py, out double pz,
                    out double rx, out double ry, out double rz, out double rw);
                return new SdfInertial
                {
                    Mass = mass,
                    Ixx = ixx, Iyy = iyy, Izz = izz,
                    Ixy = ixy, Ixz = ixz, Iyz = iyz,
                    Pose = new SdfPose3d(px, py, pz, rx, ry, rz, rw),
                };
            }
        }

        // --- Visuals ---

        /// <summary>Number of visuals.</summary>
        public ulong VisualCount => NativeMethods.sdf_link_visual_count(_ptr);

        /// <summary>Get a visual by index.</summary>
        public SdfVisual? VisualByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_link_visual_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfVisual(ptr);
        }

        /// <summary>Get a visual by name.</summary>
        public SdfVisual? VisualByName(string name)
        {
            IntPtr ptr = NativeMethods.sdf_link_visual_by_name(_ptr, name);
            return ptr == IntPtr.Zero ? null : new SdfVisual(ptr);
        }

        // --- Collisions ---

        /// <summary>Number of collisions.</summary>
        public ulong CollisionCount => NativeMethods.sdf_link_collision_count(_ptr);

        /// <summary>Get a collision by index.</summary>
        public SdfCollision? CollisionByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_link_collision_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfCollision(ptr);
        }

        /// <summary>Get a collision by name.</summary>
        public SdfCollision? CollisionByName(string name)
        {
            IntPtr ptr = NativeMethods.sdf_link_collision_by_name(_ptr, name);
            return ptr == IntPtr.Zero ? null : new SdfCollision(ptr);
        }

        // --- Sensors ---

        /// <summary>Number of sensors.</summary>
        public ulong SensorCount => NativeMethods.sdf_link_sensor_count(_ptr);

        /// <summary>Get a sensor by index.</summary>
        public SdfSensor? SensorByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_link_sensor_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfSensor(ptr);
        }

        /// <summary>Get a sensor by name.</summary>
        public SdfSensor? SensorByName(string name)
        {
            IntPtr ptr = NativeMethods.sdf_link_sensor_by_name(_ptr, name);
            return ptr == IntPtr.Zero ? null : new SdfSensor(ptr);
        }

        // --- Lights ---

        /// <summary>Number of lights.</summary>
        public ulong LightCount => NativeMethods.sdf_link_light_count(_ptr);

        /// <summary>Get a light by index.</summary>
        public SdfLight? LightByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_link_light_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfLight(ptr);
        }

        public override string ToString() => $"Link(\"{Name}\")";
    }
}

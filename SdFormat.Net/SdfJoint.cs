// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF joint. Wraps sdf::Joint.
    /// </summary>
    public sealed class SdfJoint
    {
        private readonly IntPtr _ptr;

        internal SdfJoint(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Name of the joint.</summary>
        public string Name =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_joint_name(_ptr));

        /// <summary>Type of the joint.</summary>
        public JointType Type => (JointType)NativeMethods.sdf_joint_type(_ptr);

        /// <summary>Name of the parent link.</summary>
        public string ParentName =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_joint_parent_name(_ptr));

        /// <summary>Name of the child link.</summary>
        public string ChildName =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_joint_child_name(_ptr));

        /// <summary>Raw pose of the joint.</summary>
        public SdfPose3d RawPose
        {
            get
            {
                NativeMethods.sdf_joint_raw_pose(_ptr,
                    out double px, out double py, out double pz,
                    out double rx, out double ry, out double rz, out double rw);
                return new SdfPose3d(px, py, pz, rx, ry, rz, rw);
            }
        }

        /// <summary>The frame this joint's pose is relative to.</summary>
        public string PoseRelativeTo =>
            NativeStringHelper.ConsumeStringOrEmpty(
                NativeMethods.sdf_joint_pose_relative_to(_ptr));

        /// <summary>Thread pitch (for screw joints).</summary>
        public double ThreadPitch => NativeMethods.sdf_joint_thread_pitch(_ptr);

        /// <summary>Screw thread pitch.</summary>
        public double ScrewThreadPitch => NativeMethods.sdf_joint_screw_thread_pitch(_ptr);

        /// <summary>
        /// Get a joint axis (0 = primary axis, 1 = secondary axis2).
        /// Returns null if the axis is not defined.
        /// </summary>
        public SdfJointAxis? Axis(uint index = 0)
        {
            IntPtr ptr = NativeMethods.sdf_joint_axis(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfJointAxis(ptr);
        }

        // --- Sensors ---

        /// <summary>Number of sensors attached to this joint.</summary>
        public ulong SensorCount => NativeMethods.sdf_joint_sensor_count(_ptr);

        /// <summary>Get a sensor by index.</summary>
        public SdfSensor? SensorByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_joint_sensor_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfSensor(ptr);
        }

        public override string ToString() => $"Joint(\"{Name}\", {Type})";
    }
}

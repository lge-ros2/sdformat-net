// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF joint axis. Wraps sdf::JointAxis.
    /// </summary>
    public sealed class SdfJointAxis
    {
        private readonly IntPtr _ptr;

        internal SdfJointAxis(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Axis direction vector in the frame given by XyzExpressedIn.</summary>
        public SdfVector3d Xyz
        {
            get
            {
                NativeMethods.sdf_joint_axis_xyz(_ptr, out double x, out double y, out double z);
                return new SdfVector3d(x, y, z);
            }
        }

        /// <summary>Frame in which the axis direction is expressed.</summary>
        public string XyzExpressedIn =>
            NativeStringHelper.ConsumeStringOrEmpty(
                NativeMethods.sdf_joint_axis_xyz_expressed_in(_ptr));

        /// <summary>Lower joint limit (rad or m).</summary>
        public double Lower => NativeMethods.sdf_joint_axis_lower(_ptr);

        /// <summary>Upper joint limit (rad or m).</summary>
        public double Upper => NativeMethods.sdf_joint_axis_upper(_ptr);

        /// <summary>Maximum effort (force or torque).</summary>
        public double Effort => NativeMethods.sdf_joint_axis_effort(_ptr);

        /// <summary>Maximum velocity (rad/s or m/s).</summary>
        public double MaxVelocity => NativeMethods.sdf_joint_axis_max_velocity(_ptr);

        /// <summary>Joint damping coefficient.</summary>
        public double Damping => NativeMethods.sdf_joint_axis_damping(_ptr);

        /// <summary>Joint friction.</summary>
        public double Friction => NativeMethods.sdf_joint_axis_friction(_ptr);

        /// <summary>Spring reference position.</summary>
        public double SpringReference => NativeMethods.sdf_joint_axis_spring_reference(_ptr);

        /// <summary>Spring stiffness.</summary>
        public double SpringStiffness => NativeMethods.sdf_joint_axis_spring_stiffness(_ptr);

        /// <summary>Joint stiffness.</summary>
        public double Stiffness => NativeMethods.sdf_joint_axis_stiffness(_ptr);

        /// <summary>Joint dissipation.</summary>
        public double Dissipation => NativeMethods.sdf_joint_axis_dissipation(_ptr);

        public override string ToString() => $"JointAxis(xyz={Xyz})";
    }
}

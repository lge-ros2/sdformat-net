// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF model. Wraps sdf::Model.
    /// Non-owning wrapper — lifetime is managed by the parent.
    /// </summary>
    public sealed class SdfModel
    {
        private readonly IntPtr _ptr;

        internal SdfModel(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Name of the model.</summary>
        public string Name =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_model_name(_ptr));

        /// <summary>Whether the model is static.</summary>
        public bool IsStatic => NativeMethods.sdf_model_static(_ptr) != 0;

        /// <summary>Whether self-collision is enabled.</summary>
        public bool SelfCollide => NativeMethods.sdf_model_self_collide(_ptr) != 0;

        /// <summary>Whether auto-disable is allowed.</summary>
        public bool AllowAutoDisable => NativeMethods.sdf_model_allow_auto_disable(_ptr) != 0;

        /// <summary>Whether wind effects are enabled.</summary>
        public bool EnableWind => NativeMethods.sdf_model_enable_wind(_ptr) != 0;

        /// <summary>Raw pose of the model.</summary>
        public SdfPose3d RawPose
        {
            get
            {
                NativeMethods.sdf_model_raw_pose(_ptr,
                    out double px, out double py, out double pz,
                    out double rx, out double ry, out double rz, out double rw);
                return new SdfPose3d(px, py, pz, rx, ry, rz, rw);
            }
        }

        /// <summary>Name of the canonical link.</summary>
        public string CanonicalLinkName =>
            NativeStringHelper.ConsumeStringOrEmpty(
                NativeMethods.sdf_model_canonical_link_name(_ptr));

        /// <summary>The frame this model's pose is relative to.</summary>
        public string PoseRelativeTo =>
            NativeStringHelper.ConsumeStringOrEmpty(
                NativeMethods.sdf_model_pose_relative_to(_ptr));

        /// <summary>URI associated with this model.</summary>
        public string Uri =>
            NativeStringHelper.ConsumeStringOrEmpty(
                NativeMethods.sdf_model_uri(_ptr));

        // --- Links ---

        /// <summary>Number of links.</summary>
        public ulong LinkCount => NativeMethods.sdf_model_link_count(_ptr);

        /// <summary>Get a link by index.</summary>
        public SdfLink? LinkByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_model_link_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfLink(ptr);
        }

        /// <summary>Get a link by name.</summary>
        public SdfLink? LinkByName(string name)
        {
            IntPtr ptr = NativeMethods.sdf_model_link_by_name(_ptr, name);
            return ptr == IntPtr.Zero ? null : new SdfLink(ptr);
        }

        // --- Joints ---

        /// <summary>Number of joints.</summary>
        public ulong JointCount => NativeMethods.sdf_model_joint_count(_ptr);

        /// <summary>Get a joint by index.</summary>
        public SdfJoint? JointByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_model_joint_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfJoint(ptr);
        }

        /// <summary>Get a joint by name.</summary>
        public SdfJoint? JointByName(string name)
        {
            IntPtr ptr = NativeMethods.sdf_model_joint_by_name(_ptr, name);
            return ptr == IntPtr.Zero ? null : new SdfJoint(ptr);
        }

        // --- Frames ---

        /// <summary>Number of explicit frames.</summary>
        public ulong FrameCount => NativeMethods.sdf_model_frame_count(_ptr);

        /// <summary>Get a frame by index.</summary>
        public SdfFrame? FrameByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_model_frame_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfFrame(ptr);
        }

        // --- Nested Models ---

        /// <summary>Number of nested models.</summary>
        public ulong NestedModelCount => NativeMethods.sdf_model_nested_model_count(_ptr);

        /// <summary>Get a nested model by index.</summary>
        public SdfModel? NestedModelByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_model_nested_model_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfModel(ptr);
        }

        /// <summary>Get a nested model by name.</summary>
        public SdfModel? NestedModelByName(string name)
        {
            IntPtr ptr = NativeMethods.sdf_model_nested_model_by_name(_ptr, name);
            return ptr == IntPtr.Zero ? null : new SdfModel(ptr);
        }

        public override string ToString() => $"Model(\"{Name}\")";
    }
}

// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF world. Wraps sdf::World.
    /// This is a non-owning wrapper — lifetime is managed by SdfRoot.
    /// </summary>
    public sealed class SdfWorld
    {
        private readonly IntPtr _ptr;

        internal SdfWorld(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Name of the world.</summary>
        public string Name =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_world_name(_ptr));

        /// <summary>Gravity vector.</summary>
        public SdfVector3d Gravity
        {
            get
            {
                NativeMethods.sdf_world_gravity(_ptr, out double x, out double y, out double z);
                return new SdfVector3d(x, y, z);
            }
        }

        /// <summary>Magnetic field vector.</summary>
        public SdfVector3d MagneticField
        {
            get
            {
                NativeMethods.sdf_world_magnetic_field(_ptr, out double x, out double y, out double z);
                return new SdfVector3d(x, y, z);
            }
        }

        /// <summary>Number of models.</summary>
        public ulong ModelCount => NativeMethods.sdf_world_model_count(_ptr);

        /// <summary>Get a model by index.</summary>
        public SdfModel? ModelByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_world_model_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfModel(ptr);
        }

        /// <summary>Get a model by name.</summary>
        public SdfModel? ModelByName(string name)
        {
            IntPtr ptr = NativeMethods.sdf_world_model_by_name(_ptr, name);
            return ptr == IntPtr.Zero ? null : new SdfModel(ptr);
        }

        /// <summary>Number of lights.</summary>
        public ulong LightCount => NativeMethods.sdf_world_light_count(_ptr);

        /// <summary>Get a light by index.</summary>
        public SdfLight? LightByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_world_light_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfLight(ptr);
        }

        /// <summary>Number of explicit frames.</summary>
        public ulong FrameCount => NativeMethods.sdf_world_frame_count(_ptr);

        /// <summary>Get a frame by index.</summary>
        public SdfFrame? FrameByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_world_frame_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfFrame(ptr);
        }

        /// <summary>Number of joints.</summary>
        public ulong JointCount => NativeMethods.sdf_world_joint_count(_ptr);

        /// <summary>Get a joint by index.</summary>
        public SdfJoint? JointByIndex(ulong index)
        {
            IntPtr ptr = NativeMethods.sdf_world_joint_by_index(_ptr, index);
            return ptr == IntPtr.Zero ? null : new SdfJoint(ptr);
        }

        /// <summary>Number of physics profiles.</summary>
        public ulong PhysicsCount => NativeMethods.sdf_world_physics_count(_ptr);

        /// <summary>Number of actors.</summary>
        public ulong ActorCount => NativeMethods.sdf_world_actor_count(_ptr);

        public override string ToString() => $"World(\"{Name}\")";
    }
}

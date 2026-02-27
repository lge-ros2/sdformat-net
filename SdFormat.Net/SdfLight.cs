// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF light element. Wraps sdf::Light.
    /// </summary>
    public sealed class SdfLight
    {
        private readonly IntPtr _ptr;

        internal SdfLight(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Name of the light.</summary>
        public string Name =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_light_name(_ptr));

        /// <summary>Light type.</summary>
        public LightType Type => (LightType)NativeMethods.sdf_light_type(_ptr);

        /// <summary>Raw pose of the light.</summary>
        public SdfPose3d RawPose
        {
            get
            {
                NativeMethods.sdf_light_raw_pose(_ptr,
                    out double px, out double py, out double pz,
                    out double rx, out double ry, out double rz, out double rw);
                return new SdfPose3d(px, py, pz, rx, ry, rz, rw);
            }
        }

        /// <summary>Diffuse color.</summary>
        public SdfColor Diffuse
        {
            get
            {
                NativeMethods.sdf_light_diffuse(_ptr, out float r, out float g, out float b, out float a);
                return new SdfColor(r, g, b, a);
            }
        }

        /// <summary>Specular color.</summary>
        public SdfColor Specular
        {
            get
            {
                NativeMethods.sdf_light_specular(_ptr, out float r, out float g, out float b, out float a);
                return new SdfColor(r, g, b, a);
            }
        }

        /// <summary>Light intensity.</summary>
        public double Intensity => NativeMethods.sdf_light_intensity(_ptr);

        /// <summary>Attenuation range.</summary>
        public double Range => NativeMethods.sdf_light_range(_ptr);

        /// <summary>Linear attenuation factor.</summary>
        public double LinearAttenuation => NativeMethods.sdf_light_linear_attenuation(_ptr);

        /// <summary>Constant attenuation factor.</summary>
        public double ConstantAttenuation => NativeMethods.sdf_light_constant_attenuation(_ptr);

        /// <summary>Quadratic attenuation factor.</summary>
        public double QuadraticAttenuation => NativeMethods.sdf_light_quadratic_attenuation(_ptr);

        /// <summary>Whether the light casts shadows.</summary>
        public bool CastShadows => NativeMethods.sdf_light_cast_shadows(_ptr) != 0;

        /// <summary>Direction (for directional and spot lights).</summary>
        public SdfVector3d Direction
        {
            get
            {
                NativeMethods.sdf_light_direction(_ptr, out double x, out double y, out double z);
                return new SdfVector3d(x, y, z);
            }
        }

        /// <summary>
        /// Spot light parameters. Only meaningful for spot lights.
        /// </summary>
        public (double InnerAngle, double OuterAngle, double Falloff) Spot
        {
            get
            {
                NativeMethods.sdf_light_spot(_ptr,
                    out double inner, out double outer, out double falloff);
                return (inner, outer, falloff);
            }
        }

        public override string ToString() => $"Light(\"{Name}\", {Type})";
    }
}

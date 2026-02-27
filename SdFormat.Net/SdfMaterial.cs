// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF material. Wraps sdf::Material.
    /// </summary>
    public sealed class SdfMaterial
    {
        private readonly IntPtr _ptr;

        internal SdfMaterial(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Ambient color.</summary>
        public SdfColor Ambient
        {
            get
            {
                NativeMethods.sdf_material_ambient(_ptr, out float r, out float g, out float b, out float a);
                return new SdfColor(r, g, b, a);
            }
        }

        /// <summary>Diffuse color.</summary>
        public SdfColor Diffuse
        {
            get
            {
                NativeMethods.sdf_material_diffuse(_ptr, out float r, out float g, out float b, out float a);
                return new SdfColor(r, g, b, a);
            }
        }

        /// <summary>Specular color.</summary>
        public SdfColor Specular
        {
            get
            {
                NativeMethods.sdf_material_specular(_ptr, out float r, out float g, out float b, out float a);
                return new SdfColor(r, g, b, a);
            }
        }

        /// <summary>Emissive color.</summary>
        public SdfColor Emissive
        {
            get
            {
                NativeMethods.sdf_material_emissive(_ptr, out float r, out float g, out float b, out float a);
                return new SdfColor(r, g, b, a);
            }
        }

        /// <summary>Shininess value.</summary>
        public double Shininess => NativeMethods.sdf_material_shininess(_ptr);

        /// <summary>Render order.</summary>
        public double RenderOrder => NativeMethods.sdf_material_render_order(_ptr);

        /// <summary>Whether lighting is enabled.</summary>
        public bool Lighting => NativeMethods.sdf_material_lighting(_ptr) != 0;

        /// <summary>Whether the material is double-sided.</summary>
        public bool DoubleSided => NativeMethods.sdf_material_double_sided(_ptr) != 0;

        /// <summary>Script URI for Ogre materials.</summary>
        public string ScriptUri =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_material_script_uri(_ptr));

        /// <summary>Script name for Ogre materials.</summary>
        public string ScriptName =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_material_script_name(_ptr));

        /// <summary>Shader type.</summary>
        public ShaderType Shader => (ShaderType)NativeMethods.sdf_material_shader_type(_ptr);

        /// <summary>Normal map filename.</summary>
        public string NormalMap =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_material_normal_map(_ptr));

        public override string ToString() => $"Material(diffuse={Diffuse})";
    }
}

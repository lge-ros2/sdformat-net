// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF geometry element. Wraps sdf::Geometry.
    /// Provides typed accessors for each shape variant.
    /// </summary>
    public sealed class SdfGeometry
    {
        private readonly IntPtr _ptr;

        internal SdfGeometry(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>The geometry type.</summary>
        public GeometryType Type => (GeometryType)NativeMethods.sdf_geometry_type(_ptr);

        // --- Box ---

        /// <summary>Box size (X, Y, Z). Only valid when Type == Box.</summary>
        public SdfVector3d BoxSize
        {
            get
            {
                NativeMethods.sdf_geometry_box_size(_ptr, out double x, out double y, out double z);
                return new SdfVector3d(x, y, z);
            }
        }

        // --- Sphere ---

        /// <summary>Sphere radius. Only valid when Type == Sphere.</summary>
        public double SphereRadius => NativeMethods.sdf_geometry_sphere_radius(_ptr);

        // --- Cylinder ---

        /// <summary>Cylinder radius. Only valid when Type == Cylinder.</summary>
        public double CylinderRadius
        {
            get
            {
                NativeMethods.sdf_geometry_cylinder(_ptr, out double radius, out _);
                return radius;
            }
        }

        /// <summary>Cylinder length. Only valid when Type == Cylinder.</summary>
        public double CylinderLength
        {
            get
            {
                NativeMethods.sdf_geometry_cylinder(_ptr, out _, out double length);
                return length;
            }
        }

        // --- Capsule ---

        /// <summary>Capsule radius. Only valid when Type == Capsule.</summary>
        public double CapsuleRadius
        {
            get
            {
                NativeMethods.sdf_geometry_capsule(_ptr, out double radius, out _);
                return radius;
            }
        }

        /// <summary>Capsule length. Only valid when Type == Capsule.</summary>
        public double CapsuleLength
        {
            get
            {
                NativeMethods.sdf_geometry_capsule(_ptr, out _, out double length);
                return length;
            }
        }

        // --- Cone ---

        /// <summary>Cone radius. Only valid when Type == Cone.</summary>
        public double ConeRadius
        {
            get
            {
                NativeMethods.sdf_geometry_cone(_ptr, out double radius, out _);
                return radius;
            }
        }

        /// <summary>Cone length. Only valid when Type == Cone.</summary>
        public double ConeLength
        {
            get
            {
                NativeMethods.sdf_geometry_cone(_ptr, out _, out double length);
                return length;
            }
        }

        // --- Ellipsoid ---

        /// <summary>Ellipsoid radii (X, Y, Z). Only valid when Type == Ellipsoid.</summary>
        public SdfVector3d EllipsoidRadii
        {
            get
            {
                NativeMethods.sdf_geometry_ellipsoid_radii(_ptr, out double x, out double y, out double z);
                return new SdfVector3d(x, y, z);
            }
        }

        // --- Plane ---

        /// <summary>Plane normal. Only valid when Type == Plane.</summary>
        public SdfVector3d PlaneNormal
        {
            get
            {
                NativeMethods.sdf_geometry_plane(_ptr,
                    out double nx, out double ny, out double nz,
                    out _, out _);
                return new SdfVector3d(nx, ny, nz);
            }
        }

        /// <summary>Plane size (width, height). Only valid when Type == Plane.</summary>
        public (double Width, double Height) PlaneSize
        {
            get
            {
                NativeMethods.sdf_geometry_plane(_ptr,
                    out _, out _, out _,
                    out double sx, out double sy);
                return (sx, sy);
            }
        }

        // --- Mesh ---

        /// <summary>Mesh URI. Only valid when Type == Mesh.</summary>
        public string? MeshUri =>
            NativeStringHelper.ConsumeString(NativeMethods.sdf_geometry_mesh_uri(_ptr));

        /// <summary>Resolved mesh file path. Only valid when Type == Mesh.</summary>
        public string? MeshFilePath =>
            NativeStringHelper.ConsumeString(NativeMethods.sdf_geometry_mesh_file_path(_ptr));

        /// <summary>Mesh scale. Only valid when Type == Mesh.</summary>
        public SdfVector3d MeshScale
        {
            get
            {
                NativeMethods.sdf_geometry_mesh_scale(_ptr, out double x, out double y, out double z);
                return new SdfVector3d(x, y, z);
            }
        }

        /// <summary>Submesh name. Only valid when Type == Mesh.</summary>
        public string? MeshSubmesh =>
            NativeStringHelper.ConsumeString(NativeMethods.sdf_geometry_mesh_submesh(_ptr));

        /// <summary>Whether the submesh should be centered. Only valid when Type == Mesh.</summary>
        public bool MeshCenterSubmesh => NativeMethods.sdf_geometry_mesh_center_submesh(_ptr) != 0;

        public override string ToString() => $"Geometry({Type})";
    }
}

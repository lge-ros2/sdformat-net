// Copyright (c) 2026 LGE-ROS2 — MIT License
// Lightweight math structs compatible with Unity's Vector3/Quaternion/Pose.

using System.Runtime.InteropServices;

namespace SdFormat
{
    /// <summary>
    /// 3D vector (double precision). Maps to gz::math::Vector3d.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SdfVector3d
    {
        public double X;
        public double Y;
        public double Z;

        public SdfVector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() => $"({X:F4}, {Y:F4}, {Z:F4})";

        public static SdfVector3d Zero => new SdfVector3d(0, 0, 0);
        public static SdfVector3d One => new SdfVector3d(1, 1, 1);
        public static SdfVector3d Up => new SdfVector3d(0, 0, 1);
    }

    /// <summary>
    /// Quaternion (double precision, XYZW order). Maps to gz::math::Quaterniond.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SdfQuaterniond
    {
        public double X;
        public double Y;
        public double Z;
        public double W;

        public SdfQuaterniond(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override string ToString() => $"({X:F4}, {Y:F4}, {Z:F4}, {W:F4})";

        public static SdfQuaterniond Identity => new SdfQuaterniond(0, 0, 0, 1);
    }

    /// <summary>
    /// 3D pose (position + orientation). Maps to gz::math::Pose3d.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SdfPose3d
    {
        public SdfVector3d Position;
        public SdfQuaterniond Rotation;

        public SdfPose3d(SdfVector3d position, SdfQuaterniond rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public SdfPose3d(double px, double py, double pz,
                         double rx, double ry, double rz, double rw)
        {
            Position = new SdfVector3d(px, py, pz);
            Rotation = new SdfQuaterniond(rx, ry, rz, rw);
        }

        public override string ToString() =>
            $"Pos{Position} Rot{Rotation}";

        public static SdfPose3d Zero =>
            new SdfPose3d(SdfVector3d.Zero, SdfQuaterniond.Identity);
    }

    /// <summary>
    /// RGBA color (float precision). Maps to gz::math::Color.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SdfColor
    {
        public float R;
        public float G;
        public float B;
        public float A;

        public SdfColor(float r, float g, float b, float a = 1.0f)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public override string ToString() => $"({R:F3}, {G:F3}, {B:F3}, {A:F3})";

        public static SdfColor White => new SdfColor(1, 1, 1, 1);
        public static SdfColor Black => new SdfColor(0, 0, 0, 1);
    }

    /// <summary>
    /// Inertial properties of a link — mass, inertia tensor, and pose.
    /// </summary>
    public struct SdfInertial
    {
        public double Mass;
        public double Ixx;
        public double Iyy;
        public double Izz;
        public double Ixy;
        public double Ixz;
        public double Iyz;
        public SdfPose3d Pose;
    }
}

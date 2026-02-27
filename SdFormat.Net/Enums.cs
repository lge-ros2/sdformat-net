// Copyright (c) 2026 LGE-ROS2 — MIT License

namespace SdFormat
{
    /// <summary>
    /// Geometry type for visual/collision shapes. Maps to sdf::GeometryType.
    /// </summary>
    public enum GeometryType
    {
        Empty = 0,
        Box = 1,
        Cylinder = 2,
        Plane = 3,
        Sphere = 4,
        Mesh = 5,
        Heightmap = 6,
        Capsule = 7,
        Ellipsoid = 8,
        Polyline = 9,
        Cone = 10,
    }

    /// <summary>
    /// Joint type. Maps to sdf::JointType.
    /// </summary>
    public enum JointType
    {
        Invalid = 0,
        Ball = 1,
        Continuous = 2,
        Fixed = 3,
        Gearbox = 4,
        Prismatic = 5,
        Revolute = 6,
        Revolute2 = 7,
        Screw = 8,
        Universal = 9,
    }

    /// <summary>
    /// Sensor type. Maps to sdf::SensorType.
    /// </summary>
    public enum SensorType
    {
        None = 0,
        Altimeter = 1,
        Camera = 2,
        Contact = 3,
        DepthCamera = 4,
        ForceTorque = 5,
        Gps = 6,
        Gpu_Lidar = 7,
        Imu = 8,
        LogicalCamera = 9,
        Magnetometer = 10,
        Multicamera = 11,
        Lidar = 12,
        Rfid = 13,
        RfidTag = 14,
        Sonar = 15,
        WirelessReceiver = 16,
        WirelessTransmitter = 17,
        AirPressure = 18,
        RgbdCamera = 19,
        ThermalCamera = 20,
        NavSat = 21,
        SegmentationCamera = 22,
        BoundingBoxCamera = 23,
        CustomCamera = 24,
        WideAngleCamera = 25,
        AirSpeed = 26,
    }

    /// <summary>
    /// Light type. Maps to sdf::LightType.
    /// </summary>
    public enum LightType
    {
        Invalid = 0,
        Point = 1,
        Directional = 2,
        Spot = 3,
    }

    /// <summary>
    /// Material shader type. Maps to sdf::ShaderType.
    /// </summary>
    public enum ShaderType
    {
        Pixel = 0,
        Vertex = 1,
        NormalMapObjectSpace = 2,
        NormalMapTangentSpace = 3,
    }
}

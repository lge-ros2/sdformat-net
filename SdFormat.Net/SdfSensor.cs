// Copyright (c) 2026 LGE-ROS2 — MIT License

using System;
using SdFormat.Interop;

namespace SdFormat
{
    /// <summary>
    /// An SDF sensor. Wraps sdf::Sensor.
    /// </summary>
    public sealed class SdfSensor
    {
        private readonly IntPtr _ptr;

        internal SdfSensor(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>Name of the sensor.</summary>
        public string Name =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_sensor_name(_ptr));

        /// <summary>Sensor type enum.</summary>
        public SensorType Type => (SensorType)NativeMethods.sdf_sensor_type(_ptr);

        /// <summary>Sensor type as a string.</summary>
        public string TypeString =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_sensor_type_string(_ptr));

        /// <summary>Update rate in Hz.</summary>
        public double UpdateRate => NativeMethods.sdf_sensor_update_rate(_ptr);

        /// <summary>Topic name for the sensor.</summary>
        public string Topic =>
            NativeStringHelper.ConsumeStringOrEmpty(NativeMethods.sdf_sensor_topic(_ptr));

        /// <summary>Raw pose of the sensor.</summary>
        public SdfPose3d RawPose
        {
            get
            {
                NativeMethods.sdf_sensor_raw_pose(_ptr,
                    out double px, out double py, out double pz,
                    out double rx, out double ry, out double rz, out double rw);
                return new SdfPose3d(px, py, pz, rx, ry, rz, rw);
            }
        }

        public override string ToString() => $"Sensor(\"{Name}\", {Type})";
    }
}

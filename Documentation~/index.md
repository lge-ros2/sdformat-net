# SdFormat.Net

C# .NET wrapper for [libsdformat](https://github.com/gazebosim/sdformat) — the Gazebo SDFormat parser.

## Installation

### Via Unity Package Manager (Git URL)

1. Open **Window > Package Manager**
2. Click **+ > Add package from git URL...**
3. Enter the repository URL (e.g. `https://github.com/lge-ros2/sdformat-net.git`)

### Via local path

Add to your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.lge-robotics.sdformat-net": "file:../../path/to/sdformat-net"
  }
}
```

## Prerequisites

The native shim library (`sdformat_shim.so` / `.dylib` / `.dll`) must be built and placed in the Unity project's `Assets/Plugins/` directory. See the main [README](../README.md) for build instructions.

## Quick Start

```csharp
using SdFormat;

using var root = new SdfRoot();
root.LoadFile("my_world.sdf");

var world = root.WorldByIndex(0);
Debug.Log($"World: {world.Name}, Models: {world.ModelCount}");
```

## API Reference

| Wrapper | Description |
|---------|-------------|
| `SdfRoot` | Entry point — load SDF files or strings |
| `SdfWorld` | World: gravity, models, lights |
| `SdfModel` | Model: pose, links, joints, nested models |
| `SdfLink` | Link: inertial, visuals, collisions, sensors |
| `SdfJoint` | Joint: type, parent/child, axes |
| `SdfVisual` | Visual: geometry, material |
| `SdfCollision` | Collision: geometry |
| `SdfGeometry` | Geometry: box, sphere, cylinder, mesh, etc. |
| `SdfMaterial` | Material: ambient, diffuse, specular, emissive |
| `SdfSensor` | Sensor: type, update rate, topic |
| `SdfLight` | Light: type, colors, attenuation |
| `SdfFrame` | Explicit frame: attached_to, pose |

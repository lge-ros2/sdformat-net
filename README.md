# sdformat-net

C# .NET wrapper library for [libsdformat](https://github.com/gazebosim/sdformat) — the Gazebo SDFormat parser.  
Designed for use in **Unity** and other .NET / C# projects.

## Architecture

```
sdformat-net/
├── sdformat_native/          # Thin C shim around C++ libsdformat
│   ├── CMakeLists.txt
│   └── sdformat_wrapper.cpp     # Exports flat C API via P/Invoke
├── SdFormat.Net/             # C# .NET class library (netstandard2.1)
│   ├── Enums.cs              # GeometryType, JointType, SensorType, etc.
│   ├── MathTypes.cs          # SdfVector3d, SdfQuaterniond, SdfPose3d, SdfColor
│   ├── Interop/
│   │   ├── NativeMethods.cs  # P/Invoke declarations
│   │   └── NativeStringHelper.cs
│   ├── SdfRoot.cs            # Entry point — load SDF files/strings
│   ├── SdfWorld.cs           # World wrapper
│   ├── SdfModel.cs           # Model wrapper
│   ├── SdfLink.cs            # Link wrapper  (visuals, collisions, sensors)
│   ├── SdfJoint.cs           # Joint wrapper
│   ├── SdfJointAxis.cs       # Joint axis wrapper
│   ├── SdfVisual.cs          # Visual wrapper
│   ├── SdfCollision.cs       # Collision wrapper
│   ├── SdfGeometry.cs        # Geometry wrapper (box, sphere, mesh, etc.)
│   ├── SdfMaterial.cs        # Material wrapper
│   ├── SdfSensor.cs          # Sensor wrapper
│   ├── SdfLight.cs           # Light wrapper
│   └── SdfFrame.cs           # Explicit frame wrapper
├── build_native.sh           # Build script for the native shim
└── README.md
```

## Prerequisites

### Native library (libsdformat)

Install one of the supported versions (13–16):

```bash
# Ubuntu
sudo apt install libsdformat14-dev   # or libsdformat15-dev, libsdformat16-dev

# macOS
brew tap osrf/simulation
brew install sdformat14

# Windows (conda)
conda install libsdformat --channel conda-forge
```

### .NET SDK

- .NET 6+ SDK (or Unity 2021+ for Unity projects)

## Building

### 1. Build the native shim

```bash
./build_native.sh          # Release build
./build_native.sh Debug    # Debug build
```

This produces `sdformat_wrapper.so` (Linux), `sdformat_wrapper.dylib` (macOS), 
or `sdformat_wrapper.dll` (Windows) in `sdformat_native/build/`.

### 2. Build the .NET library

```bash
cd SdFormat.Net
dotnet build
```

## Usage

```csharp
using SdFormat;

// Load an SDF file
using var root = new SdfRoot();
root.LoadFile("my_robot.sdf");

// Access the first world
var world = root.WorldByIndex(0);
Console.WriteLine($"World: {world.Name}");
Console.WriteLine($"Gravity: {world.Gravity}");

// Iterate models
for (ulong i = 0; i < world.ModelCount; i++)
{
    var model = world.ModelByIndex(i);
    Console.WriteLine($"  Model: {model.Name}, Static: {model.IsStatic}");
    Console.WriteLine($"  Pose: {model.RawPose}");

    // Iterate links
    for (ulong j = 0; j < model.LinkCount; j++)
    {
        var link = model.LinkByIndex(j);
        Console.WriteLine($"    Link: {link.Name}");
        Console.WriteLine($"    Mass: {link.Inertial.Mass:F3}");

        // Visuals
        for (ulong k = 0; k < link.VisualCount; k++)
        {
            var visual = link.VisualByIndex(k);
            Console.WriteLine($"      Visual: {visual.Name}");
            Console.WriteLine($"      Geometry: {visual.Geometry?.Type}");

            if (visual.Geometry?.Type == GeometryType.Mesh)
                Console.WriteLine($"      Mesh URI: {visual.Geometry.MeshUri}");
        }

        // Collisions
        for (ulong k = 0; k < link.CollisionCount; k++)
        {
            var collision = link.CollisionByIndex(k);
            Console.WriteLine($"      Collision: {collision.Name}, Geom: {collision.Geometry?.Type}");
        }
    }

    // Iterate joints
    for (ulong j = 0; j < model.JointCount; j++)
    {
        var joint = model.JointByIndex(j);
        Console.WriteLine($"    Joint: {joint.Name} ({joint.Type})");
        Console.WriteLine($"      Parent: {joint.ParentName} -> Child: {joint.ChildName}");

        var axis = joint.Axis(0);
        if (axis != null)
        {
            Console.WriteLine($"      Axis: {axis.Xyz}");
            Console.WriteLine($"      Limits: [{axis.Lower}, {axis.Upper}]");
        }
    }
}

// Or load from string
using var root2 = new SdfRoot();
root2.LoadString(@"
<sdf version='1.9'>
  <model name='box'>
    <link name='link'>
      <visual name='visual'>
        <geometry><box><size>1 1 1</size></box></geometry>
      </visual>
    </link>
  </model>
</sdf>");

var model2 = root2.Model;
Console.WriteLine($"Model from string: {model2?.Name}");
```

## Unity Integration

1. Build the native shim for your target platform
2. Place `sdformat_wrapper.so` / `.dylib` / `.dll` in `Assets/Plugins/`
3. Place `SdFormat.Net.dll` in `Assets/Plugins/` (or reference the project)
4. Use the `SdFormat` namespace in your scripts

### Native library placement for Unity:
```
Assets/Plugins/
├── x86_64/
│   └── sdformat_wrapper.so        # Linux
├── sdformat_wrapper.dylib          # macOS
└── sdformat_wrapper.dll            # Windows
```

## Wrapped API Coverage

| SDF Class     | C# Wrapper       | Key Properties |
|---------------|-------------------|----------------|
| `sdf::Root`   | `SdfRoot`         | Load, WorldCount, Model |
| `sdf::World`  | `SdfWorld`        | Name, Gravity, Models, Lights |
| `sdf::Model`  | `SdfModel`        | Name, Pose, Links, Joints, Nested Models |
| `sdf::Link`   | `SdfLink`         | Name, Pose, Inertial, Visuals, Collisions, Sensors |
| `sdf::Joint`  | `SdfJoint`        | Name, Type, Parent/Child, Axis |
| `sdf::JointAxis` | `SdfJointAxis` | Xyz, Limits, Damping, Friction |
| `sdf::Visual` | `SdfVisual`       | Name, Pose, Geometry, Material |
| `sdf::Collision` | `SdfCollision` | Name, Pose, Geometry |
| `sdf::Geometry` | `SdfGeometry`   | Type, Box/Sphere/Cylinder/Mesh/... |
| `sdf::Material` | `SdfMaterial`   | Ambient/Diffuse/Specular/Emissive colors |
| `sdf::Sensor` | `SdfSensor`       | Name, Type, UpdateRate, Topic |
| `sdf::Light`  | `SdfLight`        | Name, Type, Colors, Attenuation, Spot |
| `sdf::Frame`  | `SdfFrame`        | Name, AttachedTo, Pose |

## License

MIT License — see [LICENSE](LICENSE).

The upstream [libsdformat](https://github.com/gazebosim/sdformat) is licensed under Apache 2.0.

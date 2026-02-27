# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2026-02-27

### Added
- Initial release
- Native C shim (`sdformat_wrapper`) wrapping libsdformat 13–16 via P/Invoke
- C# wrappers: `SdfRoot`, `SdfWorld`, `SdfModel`, `SdfLink`, `SdfJoint`, `SdfJointAxis`, `SdfVisual`, `SdfCollision`, `SdfGeometry`, `SdfMaterial`, `SdfSensor`, `SdfLight`, `SdfFrame`
- Math types: `SdfVector3d`, `SdfQuaterniond`, `SdfPose3d`, `SdfColor`
- Enums: `GeometryType`, `JointType`, `SensorType`, `LightType`
- Unity sample: `SdfLoader` MonoBehaviour
- Build script for native shim (`build_native.sh`)

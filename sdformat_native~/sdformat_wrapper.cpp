/*
 * sdformat_wrapper.cpp
 *
 * Thin C wrapper around libsdformat C++ API for P/Invoke from .NET/Unity.
 * Exposes opaque handle pointers and flat C functions.
 *
 * Copyright (c) 2026 LGE-ROS2 — MIT License
 */

#include <cstring>
#include <cstdlib>
#include <string>
#include <vector>

#include <sdf/sdf.hh>

// ============================================================================
// Helper macros
// ============================================================================
#define EXPORT extern "C" __attribute__((visibility("default")))

// Allocate a C-style copy of a std::string the caller must free with sdf_free_string.
static char *to_c_string(const std::string &s)
{
    char *buf = static_cast<char *>(std::malloc(s.size() + 1));
    if (buf)
    {
        std::memcpy(buf, s.c_str(), s.size() + 1);
    }
    return buf;
}

// ============================================================================
// Memory helpers
// ============================================================================
EXPORT void sdf_free_string(char *str)
{
    std::free(str);
}

// ============================================================================
// sdf::Root
// ============================================================================
EXPORT sdf::Root *sdf_root_create()
{
    return new sdf::Root();
}

EXPORT void sdf_root_destroy(sdf::Root *root)
{
    delete root;
}

EXPORT int sdf_root_load_file(sdf::Root *root, const char *filename, char **error_out)
{
    if (!root || !filename)
        return -1;
    auto errors = root->Load(std::string(filename));
    if (!errors.empty())
    {
        std::string msg;
        for (auto &e : errors)
        {
            if (!msg.empty()) msg += "\n";
            msg += e.Message();
        }
        if (error_out)
            *error_out = to_c_string(msg);
        return static_cast<int>(errors.size());
    }
    return 0;
}

EXPORT int sdf_root_load_string(sdf::Root *root, const char *sdf_string, char **error_out)
{
    if (!root || !sdf_string)
        return -1;
    auto errors = root->LoadSdfString(std::string(sdf_string));
    if (!errors.empty())
    {
        std::string msg;
        for (auto &e : errors)
        {
            if (!msg.empty()) msg += "\n";
            msg += e.Message();
        }
        if (error_out)
            *error_out = to_c_string(msg);
        return static_cast<int>(errors.size());
    }
    return 0;
}

EXPORT char *sdf_root_version(const sdf::Root *root)
{
    if (!root) return nullptr;
    return to_c_string(root->Version());
}

EXPORT uint64_t sdf_root_world_count(const sdf::Root *root)
{
    if (!root) return 0;
    return root->WorldCount();
}

EXPORT const sdf::World *sdf_root_world_by_index(const sdf::Root *root, uint64_t index)
{
    if (!root) return nullptr;
    return root->WorldByIndex(index);
}

EXPORT const sdf::World *sdf_root_world_by_name(const sdf::Root *root, const char *name)
{
    if (!root || !name) return nullptr;
    return root->WorldByName(std::string(name));
}

EXPORT const sdf::Model *sdf_root_model(const sdf::Root *root)
{
    if (!root) return nullptr;
    return root->Model();
}

// ============================================================================
// sdf::World
// ============================================================================
EXPORT char *sdf_world_name(const sdf::World *world)
{
    if (!world) return nullptr;
    return to_c_string(world->Name());
}

EXPORT void sdf_world_gravity(const sdf::World *world, double *x, double *y, double *z)
{
    if (!world) return;
    auto g = world->Gravity();
    if (x) *x = g.X();
    if (y) *y = g.Y();
    if (z) *z = g.Z();
}

EXPORT void sdf_world_magnetic_field(const sdf::World *world, double *x, double *y, double *z)
{
    if (!world) return;
    auto m = world->MagneticField();
    if (x) *x = m.X();
    if (y) *y = m.Y();
    if (z) *z = m.Z();
}

EXPORT uint64_t sdf_world_model_count(const sdf::World *world)
{
    if (!world) return 0;
    return world->ModelCount();
}

EXPORT const sdf::Model *sdf_world_model_by_index(const sdf::World *world, uint64_t index)
{
    if (!world) return nullptr;
    return world->ModelByIndex(index);
}

EXPORT const sdf::Model *sdf_world_model_by_name(const sdf::World *world, const char *name)
{
    if (!world || !name) return nullptr;
    return world->ModelByName(std::string(name));
}

EXPORT uint64_t sdf_world_light_count(const sdf::World *world)
{
    if (!world) return 0;
    return world->LightCount();
}

EXPORT const sdf::Light *sdf_world_light_by_index(const sdf::World *world, uint64_t index)
{
    if (!world) return nullptr;
    return world->LightByIndex(index);
}

EXPORT uint64_t sdf_world_frame_count(const sdf::World *world)
{
    if (!world) return 0;
    return world->FrameCount();
}

EXPORT const sdf::Frame *sdf_world_frame_by_index(const sdf::World *world, uint64_t index)
{
    if (!world) return nullptr;
    return world->FrameByIndex(index);
}

EXPORT uint64_t sdf_world_joint_count(const sdf::World *world)
{
    if (!world) return 0;
    return world->JointCount();
}

EXPORT const sdf::Joint *sdf_world_joint_by_index(const sdf::World *world, uint64_t index)
{
    if (!world) return nullptr;
    return world->JointByIndex(index);
}

EXPORT uint64_t sdf_world_physics_count(const sdf::World *world)
{
    if (!world) return 0;
    return world->PhysicsCount();
}

EXPORT uint64_t sdf_world_actor_count(const sdf::World *world)
{
    if (!world) return 0;
    return world->ActorCount();
}

// ============================================================================
// sdf::Model
// ============================================================================
EXPORT char *sdf_model_name(const sdf::Model *model)
{
    if (!model) return nullptr;
    return to_c_string(model->Name());
}

EXPORT int sdf_model_static(const sdf::Model *model)
{
    if (!model) return 0;
    return model->Static() ? 1 : 0;
}

EXPORT int sdf_model_self_collide(const sdf::Model *model)
{
    if (!model) return 0;
    return model->SelfCollide() ? 1 : 0;
}

EXPORT int sdf_model_allow_auto_disable(const sdf::Model *model)
{
    if (!model) return 0;
    return model->AllowAutoDisable() ? 1 : 0;
}

EXPORT int sdf_model_enable_wind(const sdf::Model *model)
{
    if (!model) return 0;
    return model->EnableWind() ? 1 : 0;
}

EXPORT void sdf_model_raw_pose(const sdf::Model *model,
                                double *px, double *py, double *pz,
                                double *rx, double *ry, double *rz, double *rw)
{
    if (!model) return;
    auto &pose = model->RawPose();
    if (px) *px = pose.Pos().X();
    if (py) *py = pose.Pos().Y();
    if (pz) *pz = pose.Pos().Z();
    if (rx) *rx = pose.Rot().X();
    if (ry) *ry = pose.Rot().Y();
    if (rz) *rz = pose.Rot().Z();
    if (rw) *rw = pose.Rot().W();
}

EXPORT char *sdf_model_canonical_link_name(const sdf::Model *model)
{
    if (!model) return nullptr;
    return to_c_string(model->CanonicalLinkName());
}

EXPORT char *sdf_model_pose_relative_to(const sdf::Model *model)
{
    if (!model) return nullptr;
    return to_c_string(model->PoseRelativeTo());
}

EXPORT char *sdf_model_uri(const sdf::Model *model)
{
    if (!model) return nullptr;
    return to_c_string(model->Uri());
}

EXPORT uint64_t sdf_model_link_count(const sdf::Model *model)
{
    if (!model) return 0;
    return model->LinkCount();
}

EXPORT const sdf::Link *sdf_model_link_by_index(const sdf::Model *model, uint64_t index)
{
    if (!model) return nullptr;
    return model->LinkByIndex(index);
}

EXPORT const sdf::Link *sdf_model_link_by_name(const sdf::Model *model, const char *name)
{
    if (!model || !name) return nullptr;
    return model->LinkByName(std::string(name));
}

EXPORT uint64_t sdf_model_joint_count(const sdf::Model *model)
{
    if (!model) return 0;
    return model->JointCount();
}

EXPORT const sdf::Joint *sdf_model_joint_by_index(const sdf::Model *model, uint64_t index)
{
    if (!model) return nullptr;
    return model->JointByIndex(index);
}

EXPORT const sdf::Joint *sdf_model_joint_by_name(const sdf::Model *model, const char *name)
{
    if (!model || !name) return nullptr;
    return model->JointByName(std::string(name));
}

EXPORT uint64_t sdf_model_frame_count(const sdf::Model *model)
{
    if (!model) return 0;
    return model->FrameCount();
}

EXPORT const sdf::Frame *sdf_model_frame_by_index(const sdf::Model *model, uint64_t index)
{
    if (!model) return nullptr;
    return model->FrameByIndex(index);
}

EXPORT uint64_t sdf_model_nested_model_count(const sdf::Model *model)
{
    if (!model) return 0;
    return model->ModelCount();
}

EXPORT const sdf::Model *sdf_model_nested_model_by_index(const sdf::Model *model, uint64_t index)
{
    if (!model) return nullptr;
    return model->ModelByIndex(index);
}

EXPORT const sdf::Model *sdf_model_nested_model_by_name(const sdf::Model *model, const char *name)
{
    if (!model || !name) return nullptr;
    return model->ModelByName(std::string(name));
}

// ============================================================================
// sdf::Link
// ============================================================================
EXPORT char *sdf_link_name(const sdf::Link *link)
{
    if (!link) return nullptr;
    return to_c_string(link->Name());
}

EXPORT void sdf_link_raw_pose(const sdf::Link *link,
                               double *px, double *py, double *pz,
                               double *rx, double *ry, double *rz, double *rw)
{
    if (!link) return;
    auto &pose = link->RawPose();
    if (px) *px = pose.Pos().X();
    if (py) *py = pose.Pos().Y();
    if (pz) *pz = pose.Pos().Z();
    if (rx) *rx = pose.Rot().X();
    if (ry) *ry = pose.Rot().Y();
    if (rz) *rz = pose.Rot().Z();
    if (rw) *rw = pose.Rot().W();
}

EXPORT char *sdf_link_pose_relative_to(const sdf::Link *link)
{
    if (!link) return nullptr;
    return to_c_string(link->PoseRelativeTo());
}

EXPORT int sdf_link_enable_wind(const sdf::Link *link)
{
    if (!link) return 0;
    return link->EnableWind() ? 1 : 0;
}

EXPORT int sdf_link_enable_gravity(const sdf::Link *link)
{
    if (!link) return 0;
    return link->EnableGravity() ? 1 : 0;
}

EXPORT int sdf_link_kinematic(const sdf::Link * /*link*/)
{
    // Kinematic() is only available in sdformat15+.
    // Return 0 (false) for older versions.
    return 0;
}

EXPORT void sdf_link_inertial(const sdf::Link *link,
                               double *mass,
                               double *ixx, double *iyy, double *izz,
                               double *ixy, double *ixz, double *iyz,
                               double *px, double *py, double *pz,
                               double *rx, double *ry, double *rz, double *rw)
{
    if (!link) return;
    auto &inertial = link->Inertial();
    if (mass) *mass = inertial.MassMatrix().Mass();
    if (ixx) *ixx = inertial.MassMatrix().Ixx();
    if (iyy) *iyy = inertial.MassMatrix().Iyy();
    if (izz) *izz = inertial.MassMatrix().Izz();
    if (ixy) *ixy = inertial.MassMatrix().Ixy();
    if (ixz) *ixz = inertial.MassMatrix().Ixz();
    if (iyz) *iyz = inertial.MassMatrix().Iyz();
    auto &pose = inertial.Pose();
    if (px) *px = pose.Pos().X();
    if (py) *py = pose.Pos().Y();
    if (pz) *pz = pose.Pos().Z();
    if (rx) *rx = pose.Rot().X();
    if (ry) *ry = pose.Rot().Y();
    if (rz) *rz = pose.Rot().Z();
    if (rw) *rw = pose.Rot().W();
}

EXPORT uint64_t sdf_link_visual_count(const sdf::Link *link)
{
    if (!link) return 0;
    return link->VisualCount();
}

EXPORT const sdf::Visual *sdf_link_visual_by_index(const sdf::Link *link, uint64_t index)
{
    if (!link) return nullptr;
    return link->VisualByIndex(index);
}

EXPORT const sdf::Visual *sdf_link_visual_by_name(const sdf::Link *link, const char *name)
{
    if (!link || !name) return nullptr;
    return link->VisualByName(std::string(name));
}

EXPORT uint64_t sdf_link_collision_count(const sdf::Link *link)
{
    if (!link) return 0;
    return link->CollisionCount();
}

EXPORT const sdf::Collision *sdf_link_collision_by_index(const sdf::Link *link, uint64_t index)
{
    if (!link) return nullptr;
    return link->CollisionByIndex(index);
}

EXPORT const sdf::Collision *sdf_link_collision_by_name(const sdf::Link *link, const char *name)
{
    if (!link || !name) return nullptr;
    return link->CollisionByName(std::string(name));
}

EXPORT uint64_t sdf_link_sensor_count(const sdf::Link *link)
{
    if (!link) return 0;
    return link->SensorCount();
}

EXPORT const sdf::Sensor *sdf_link_sensor_by_index(const sdf::Link *link, uint64_t index)
{
    if (!link) return nullptr;
    return link->SensorByIndex(index);
}

EXPORT const sdf::Sensor *sdf_link_sensor_by_name(const sdf::Link *link, const char *name)
{
    if (!link || !name) return nullptr;
    return link->SensorByName(std::string(name));
}

EXPORT uint64_t sdf_link_light_count(const sdf::Link *link)
{
    if (!link) return 0;
    return link->LightCount();
}

EXPORT const sdf::Light *sdf_link_light_by_index(const sdf::Link *link, uint64_t index)
{
    if (!link) return nullptr;
    return link->LightByIndex(index);
}

// ============================================================================
// sdf::Joint
// ============================================================================
EXPORT char *sdf_joint_name(const sdf::Joint *joint)
{
    if (!joint) return nullptr;
    return to_c_string(joint->Name());
}

EXPORT int sdf_joint_type(const sdf::Joint *joint)
{
    if (!joint) return 0;
    return static_cast<int>(joint->Type());
}

EXPORT char *sdf_joint_parent_name(const sdf::Joint *joint)
{
    if (!joint) return nullptr;
    return to_c_string(joint->ParentName());
}

EXPORT char *sdf_joint_child_name(const sdf::Joint *joint)
{
    if (!joint) return nullptr;
    return to_c_string(joint->ChildName());
}

EXPORT void sdf_joint_raw_pose(const sdf::Joint *joint,
                                double *px, double *py, double *pz,
                                double *rx, double *ry, double *rz, double *rw)
{
    if (!joint) return;
    auto &pose = joint->RawPose();
    if (px) *px = pose.Pos().X();
    if (py) *py = pose.Pos().Y();
    if (pz) *pz = pose.Pos().Z();
    if (rx) *rx = pose.Rot().X();
    if (ry) *ry = pose.Rot().Y();
    if (rz) *rz = pose.Rot().Z();
    if (rw) *rw = pose.Rot().W();
}

EXPORT char *sdf_joint_pose_relative_to(const sdf::Joint *joint)
{
    if (!joint) return nullptr;
    return to_c_string(joint->PoseRelativeTo());
}

EXPORT double sdf_joint_thread_pitch(const sdf::Joint *joint)
{
    if (!joint) return 0.0;
    return joint->ThreadPitch();
}

EXPORT double sdf_joint_screw_thread_pitch(const sdf::Joint *joint)
{
    if (!joint) return 0.0;
    return joint->ScrewThreadPitch();
}

EXPORT uint64_t sdf_joint_sensor_count(const sdf::Joint *joint)
{
    if (!joint) return 0;
    return joint->SensorCount();
}

EXPORT const sdf::Sensor *sdf_joint_sensor_by_index(const sdf::Joint *joint, uint64_t index)
{
    if (!joint) return nullptr;
    return joint->SensorByIndex(index);
}

// ============================================================================
// sdf::JointAxis
// ============================================================================
EXPORT const sdf::JointAxis *sdf_joint_axis(const sdf::Joint *joint, unsigned int index)
{
    if (!joint) return nullptr;
    return joint->Axis(index);
}

EXPORT void sdf_joint_axis_xyz(const sdf::JointAxis *axis, double *x, double *y, double *z)
{
    if (!axis) return;
    auto xyz = axis->Xyz();
    if (x) *x = xyz.X();
    if (y) *y = xyz.Y();
    if (z) *z = xyz.Z();
}

EXPORT char *sdf_joint_axis_xyz_expressed_in(const sdf::JointAxis *axis)
{
    if (!axis) return nullptr;
    return to_c_string(axis->XyzExpressedIn());
}

EXPORT double sdf_joint_axis_lower(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->Lower();
}

EXPORT double sdf_joint_axis_upper(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->Upper();
}

EXPORT double sdf_joint_axis_effort(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->Effort();
}

EXPORT double sdf_joint_axis_max_velocity(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->MaxVelocity();
}

EXPORT double sdf_joint_axis_damping(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->Damping();
}

EXPORT double sdf_joint_axis_friction(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->Friction();
}

EXPORT double sdf_joint_axis_spring_reference(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->SpringReference();
}

EXPORT double sdf_joint_axis_spring_stiffness(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->SpringStiffness();
}

EXPORT double sdf_joint_axis_stiffness(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->Stiffness();
}

EXPORT double sdf_joint_axis_dissipation(const sdf::JointAxis *axis)
{
    if (!axis) return 0.0;
    return axis->Dissipation();
}

// ============================================================================
// sdf::Visual
// ============================================================================
EXPORT char *sdf_visual_name(const sdf::Visual *visual)
{
    if (!visual) return nullptr;
    return to_c_string(visual->Name());
}

EXPORT int sdf_visual_cast_shadows(const sdf::Visual *visual)
{
    if (!visual) return 1;
    return visual->CastShadows() ? 1 : 0;
}

EXPORT float sdf_visual_transparency(const sdf::Visual *visual)
{
    if (!visual) return 0.0f;
    return visual->Transparency();
}

EXPORT void sdf_visual_raw_pose(const sdf::Visual *visual,
                                 double *px, double *py, double *pz,
                                 double *rx, double *ry, double *rz, double *rw)
{
    if (!visual) return;
    auto &pose = visual->RawPose();
    if (px) *px = pose.Pos().X();
    if (py) *py = pose.Pos().Y();
    if (pz) *pz = pose.Pos().Z();
    if (rx) *rx = pose.Rot().X();
    if (ry) *ry = pose.Rot().Y();
    if (rz) *rz = pose.Rot().Z();
    if (rw) *rw = pose.Rot().W();
}

EXPORT char *sdf_visual_pose_relative_to(const sdf::Visual *visual)
{
    if (!visual) return nullptr;
    return to_c_string(visual->PoseRelativeTo());
}

EXPORT const sdf::Geometry *sdf_visual_geometry(const sdf::Visual *visual)
{
    if (!visual) return nullptr;
    return visual->Geom();
}

EXPORT const sdf::Material *sdf_visual_material(const sdf::Visual *visual)
{
    if (!visual) return nullptr;
    return visual->Material();
}

EXPORT uint32_t sdf_visual_visibility_flags(const sdf::Visual *visual)
{
    if (!visual) return 0xFFFFFFFF;
    return visual->VisibilityFlags();
}

EXPORT int sdf_visual_has_laser_retro(const sdf::Visual *visual)
{
    if (!visual) return 0;
    return visual->HasLaserRetro() ? 1 : 0;
}

EXPORT double sdf_visual_laser_retro(const sdf::Visual *visual)
{
    if (!visual) return 0.0;
    return visual->LaserRetro();
}

// ============================================================================
// sdf::Collision
// ============================================================================
EXPORT char *sdf_collision_name(const sdf::Collision *collision)
{
    if (!collision) return nullptr;
    return to_c_string(collision->Name());
}

EXPORT void sdf_collision_raw_pose(const sdf::Collision *collision,
                                    double *px, double *py, double *pz,
                                    double *rx, double *ry, double *rz, double *rw)
{
    if (!collision) return;
    auto &pose = collision->RawPose();
    if (px) *px = pose.Pos().X();
    if (py) *py = pose.Pos().Y();
    if (pz) *pz = pose.Pos().Z();
    if (rx) *rx = pose.Rot().X();
    if (ry) *ry = pose.Rot().Y();
    if (rz) *rz = pose.Rot().Z();
    if (rw) *rw = pose.Rot().W();
}

EXPORT const sdf::Geometry *sdf_collision_geometry(const sdf::Collision *collision)
{
    if (!collision) return nullptr;
    return collision->Geom();
}

// ============================================================================
// sdf::Geometry
// ============================================================================
EXPORT int sdf_geometry_type(const sdf::Geometry *geom)
{
    if (!geom) return 0;
    return static_cast<int>(geom->Type());
}

// --- Box ---
EXPORT void sdf_geometry_box_size(const sdf::Geometry *geom, double *x, double *y, double *z)
{
    if (!geom || geom->Type() != sdf::GeometryType::BOX || !geom->BoxShape())
        return;
    auto size = geom->BoxShape()->Size();
    if (x) *x = size.X();
    if (y) *y = size.Y();
    if (z) *z = size.Z();
}

// --- Sphere ---
EXPORT double sdf_geometry_sphere_radius(const sdf::Geometry *geom)
{
    if (!geom || geom->Type() != sdf::GeometryType::SPHERE || !geom->SphereShape())
        return 0.0;
    return geom->SphereShape()->Radius();
}

// --- Cylinder ---
EXPORT void sdf_geometry_cylinder(const sdf::Geometry *geom, double *radius, double *length)
{
    if (!geom || geom->Type() != sdf::GeometryType::CYLINDER || !geom->CylinderShape())
        return;
    if (radius) *radius = geom->CylinderShape()->Radius();
    if (length) *length = geom->CylinderShape()->Length();
}

// --- Capsule ---
EXPORT void sdf_geometry_capsule(const sdf::Geometry *geom, double *radius, double *length)
{
    if (!geom || geom->Type() != sdf::GeometryType::CAPSULE || !geom->CapsuleShape())
        return;
    if (radius) *radius = geom->CapsuleShape()->Radius();
    if (length) *length = geom->CapsuleShape()->Length();
}

// --- Cone ---
EXPORT void sdf_geometry_cone(const sdf::Geometry *geom, double *radius, double *length)
{
    if (!geom || geom->Type() != sdf::GeometryType::CONE || !geom->ConeShape())
        return;
    if (radius) *radius = geom->ConeShape()->Radius();
    if (length) *length = geom->ConeShape()->Length();
}

// --- Ellipsoid ---
EXPORT void sdf_geometry_ellipsoid_radii(const sdf::Geometry *geom, double *x, double *y, double *z)
{
    if (!geom || geom->Type() != sdf::GeometryType::ELLIPSOID || !geom->EllipsoidShape())
        return;
    auto radii = geom->EllipsoidShape()->Radii();
    if (x) *x = radii.X();
    if (y) *y = radii.Y();
    if (z) *z = radii.Z();
}

// --- Plane ---
EXPORT void sdf_geometry_plane(const sdf::Geometry *geom,
                                double *nx, double *ny, double *nz,
                                double *sx, double *sy)
{
    if (!geom || geom->Type() != sdf::GeometryType::PLANE || !geom->PlaneShape())
        return;
    auto normal = geom->PlaneShape()->Normal();
    if (nx) *nx = normal.X();
    if (ny) *ny = normal.Y();
    if (nz) *nz = normal.Z();
    auto size = geom->PlaneShape()->Size();
    if (sx) *sx = size.X();
    if (sy) *sy = size.Y();
}

// --- Mesh ---
EXPORT char *sdf_geometry_mesh_uri(const sdf::Geometry *geom)
{
    if (!geom || geom->Type() != sdf::GeometryType::MESH || !geom->MeshShape())
        return nullptr;
    return to_c_string(geom->MeshShape()->Uri());
}

EXPORT char *sdf_geometry_mesh_file_path(const sdf::Geometry *geom)
{
    if (!geom || geom->Type() != sdf::GeometryType::MESH || !geom->MeshShape())
        return nullptr;
    return to_c_string(geom->MeshShape()->FilePath());
}

EXPORT void sdf_geometry_mesh_scale(const sdf::Geometry *geom, double *x, double *y, double *z)
{
    if (!geom || geom->Type() != sdf::GeometryType::MESH || !geom->MeshShape())
        return;
    auto scale = geom->MeshShape()->Scale();
    if (x) *x = scale.X();
    if (y) *y = scale.Y();
    if (z) *z = scale.Z();
}

EXPORT char *sdf_geometry_mesh_submesh(const sdf::Geometry *geom)
{
    if (!geom || geom->Type() != sdf::GeometryType::MESH || !geom->MeshShape())
        return nullptr;
    return to_c_string(geom->MeshShape()->Submesh());
}

EXPORT int sdf_geometry_mesh_center_submesh(const sdf::Geometry *geom)
{
    if (!geom || geom->Type() != sdf::GeometryType::MESH || !geom->MeshShape())
        return 0;
    return geom->MeshShape()->CenterSubmesh() ? 1 : 0;
}

// ============================================================================
// sdf::Material
// ============================================================================
EXPORT void sdf_material_ambient(const sdf::Material *mat, float *r, float *g, float *b, float *a)
{
    if (!mat) return;
    auto c = mat->Ambient();
    if (r) *r = c.R();
    if (g) *g = c.G();
    if (b) *b = c.B();
    if (a) *a = c.A();
}

EXPORT void sdf_material_diffuse(const sdf::Material *mat, float *r, float *g, float *b, float *a)
{
    if (!mat) return;
    auto c = mat->Diffuse();
    if (r) *r = c.R();
    if (g) *g = c.G();
    if (b) *b = c.B();
    if (a) *a = c.A();
}

EXPORT void sdf_material_specular(const sdf::Material *mat, float *r, float *g, float *b, float *a)
{
    if (!mat) return;
    auto c = mat->Specular();
    if (r) *r = c.R();
    if (g) *g = c.G();
    if (b) *b = c.B();
    if (a) *a = c.A();
}

EXPORT void sdf_material_emissive(const sdf::Material *mat, float *r, float *g, float *b, float *a)
{
    if (!mat) return;
    auto c = mat->Emissive();
    if (r) *r = c.R();
    if (g) *g = c.G();
    if (b) *b = c.B();
    if (a) *a = c.A();
}

EXPORT double sdf_material_shininess(const sdf::Material *mat)
{
    if (!mat) return 0.0;
    return mat->Shininess();
}

EXPORT double sdf_material_render_order(const sdf::Material *mat)
{
    if (!mat) return 0.0;
    return mat->RenderOrder();
}

EXPORT int sdf_material_lighting(const sdf::Material *mat)
{
    if (!mat) return 1;
    return mat->Lighting() ? 1 : 0;
}

EXPORT int sdf_material_double_sided(const sdf::Material *mat)
{
    if (!mat) return 0;
    return mat->DoubleSided() ? 1 : 0;
}

EXPORT char *sdf_material_script_uri(const sdf::Material *mat)
{
    if (!mat) return nullptr;
    return to_c_string(mat->ScriptUri());
}

EXPORT char *sdf_material_script_name(const sdf::Material *mat)
{
    if (!mat) return nullptr;
    return to_c_string(mat->ScriptName());
}

EXPORT int sdf_material_shader_type(const sdf::Material *mat)
{
    if (!mat) return 0;
    return static_cast<int>(mat->Shader());
}

EXPORT char *sdf_material_normal_map(const sdf::Material *mat)
{
    if (!mat) return nullptr;
    return to_c_string(mat->NormalMap());
}

// ============================================================================
// sdf::Sensor (basic properties)
// ============================================================================
EXPORT char *sdf_sensor_name(const sdf::Sensor *sensor)
{
    if (!sensor) return nullptr;
    return to_c_string(sensor->Name());
}

EXPORT int sdf_sensor_type(const sdf::Sensor *sensor)
{
    if (!sensor) return 0;
    return static_cast<int>(sensor->Type());
}

EXPORT char *sdf_sensor_type_string(const sdf::Sensor *sensor)
{
    if (!sensor) return nullptr;
    return to_c_string(sensor->TypeStr());
}

EXPORT double sdf_sensor_update_rate(const sdf::Sensor *sensor)
{
    if (!sensor) return 0.0;
    return sensor->UpdateRate();
}

EXPORT char *sdf_sensor_topic(const sdf::Sensor *sensor)
{
    if (!sensor) return nullptr;
    return to_c_string(sensor->Topic());
}

EXPORT void sdf_sensor_raw_pose(const sdf::Sensor *sensor,
                                  double *px, double *py, double *pz,
                                  double *rx, double *ry, double *rz, double *rw)
{
    if (!sensor) return;
    auto &pose = sensor->RawPose();
    if (px) *px = pose.Pos().X();
    if (py) *py = pose.Pos().Y();
    if (pz) *pz = pose.Pos().Z();
    if (rx) *rx = pose.Rot().X();
    if (ry) *ry = pose.Rot().Y();
    if (rz) *rz = pose.Rot().Z();
    if (rw) *rw = pose.Rot().W();
}

// ============================================================================
// sdf::Light (basic properties)
// ============================================================================
EXPORT char *sdf_light_name(const sdf::Light *light)
{
    if (!light) return nullptr;
    return to_c_string(light->Name());
}

EXPORT int sdf_light_type(const sdf::Light *light)
{
    if (!light) return 0;
    return static_cast<int>(light->Type());
}

EXPORT void sdf_light_raw_pose(const sdf::Light *light,
                                 double *px, double *py, double *pz,
                                 double *rx, double *ry, double *rz, double *rw)
{
    if (!light) return;
    auto &pose = light->RawPose();
    if (px) *px = pose.Pos().X();
    if (py) *py = pose.Pos().Y();
    if (pz) *pz = pose.Pos().Z();
    if (rx) *rx = pose.Rot().X();
    if (ry) *ry = pose.Rot().Y();
    if (rz) *rz = pose.Rot().Z();
    if (rw) *rw = pose.Rot().W();
}

EXPORT void sdf_light_diffuse(const sdf::Light *light, float *r, float *g, float *b, float *a)
{
    if (!light) return;
    auto c = light->Diffuse();
    if (r) *r = c.R();
    if (g) *g = c.G();
    if (b) *b = c.B();
    if (a) *a = c.A();
}

EXPORT void sdf_light_specular(const sdf::Light *light, float *r, float *g, float *b, float *a)
{
    if (!light) return;
    auto c = light->Specular();
    if (r) *r = c.R();
    if (g) *g = c.G();
    if (b) *b = c.B();
    if (a) *a = c.A();
}

EXPORT double sdf_light_intensity(const sdf::Light *light)
{
    if (!light) return 1.0;
    return light->Intensity();
}

EXPORT double sdf_light_range(const sdf::Light *light)
{
    if (!light) return 0.0;
    return light->AttenuationRange();
}

EXPORT double sdf_light_linear_attenuation(const sdf::Light *light)
{
    if (!light) return 0.0;
    return light->LinearAttenuationFactor();
}

EXPORT double sdf_light_constant_attenuation(const sdf::Light *light)
{
    if (!light) return 1.0;
    return light->ConstantAttenuationFactor();
}

EXPORT double sdf_light_quadratic_attenuation(const sdf::Light *light)
{
    if (!light) return 0.0;
    return light->QuadraticAttenuationFactor();
}

EXPORT int sdf_light_cast_shadows(const sdf::Light *light)
{
    if (!light) return 0;
    return light->CastShadows() ? 1 : 0;
}

EXPORT void sdf_light_direction(const sdf::Light *light, double *x, double *y, double *z)
{
    if (!light) return;
    auto d = light->Direction();
    if (x) *x = d.X();
    if (y) *y = d.Y();
    if (z) *z = d.Z();
}

EXPORT void sdf_light_spot(const sdf::Light *light,
                             double *inner_angle, double *outer_angle, double *falloff)
{
    if (!light) return;
    if (inner_angle) *inner_angle = light->SpotInnerAngle().Radian();
    if (outer_angle) *outer_angle = light->SpotOuterAngle().Radian();
    if (falloff) *falloff = light->SpotFalloff();
}

// ============================================================================
// sdf::Frame
// ============================================================================
EXPORT char *sdf_frame_name(const sdf::Frame *frame)
{
    if (!frame) return nullptr;
    return to_c_string(frame->Name());
}

EXPORT char *sdf_frame_attached_to(const sdf::Frame *frame)
{
    if (!frame) return nullptr;
    return to_c_string(frame->AttachedTo());
}

EXPORT void sdf_frame_raw_pose(const sdf::Frame *frame,
                                 double *px, double *py, double *pz,
                                 double *rx, double *ry, double *rz, double *rw)
{
    if (!frame) return;
    auto &pose = frame->RawPose();
    if (px) *px = pose.Pos().X();
    if (py) *py = pose.Pos().Y();
    if (pz) *pz = pose.Pos().Z();
    if (rx) *rx = pose.Rot().X();
    if (ry) *ry = pose.Rot().Y();
    if (rz) *rz = pose.Rot().Z();
    if (rw) *rw = pose.Rot().W();
}

EXPORT char *sdf_frame_pose_relative_to(const sdf::Frame *frame)
{
    if (!frame) return nullptr;
    return to_c_string(frame->PoseRelativeTo());
}

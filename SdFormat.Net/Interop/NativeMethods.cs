// Copyright (c) 2026 LGE-ROS2 — MIT License
// P/Invoke declarations for the native sdformat_shim library.

using System;
using System.Runtime.InteropServices;

namespace SdFormat.Interop
{
    internal static class NativeMethods
    {
        private const string Lib = "sdformat_shim";

        // ====================================================================
        // Memory
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_free_string(IntPtr str);

        // ====================================================================
        // Root
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_root_create();

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_root_destroy(IntPtr root);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_root_load_file(
            IntPtr root,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string filename,
            out IntPtr errorOut);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_root_load_string(
            IntPtr root,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string sdfString,
            out IntPtr errorOut);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_root_version(IntPtr root);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_root_world_count(IntPtr root);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_root_world_by_index(IntPtr root, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_root_world_by_name(
            IntPtr root,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_root_model(IntPtr root);

        // ====================================================================
        // World
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_world_name(IntPtr world);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_world_gravity(
            IntPtr world, out double x, out double y, out double z);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_world_magnetic_field(
            IntPtr world, out double x, out double y, out double z);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_world_model_count(IntPtr world);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_world_model_by_index(IntPtr world, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_world_model_by_name(
            IntPtr world,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_world_light_count(IntPtr world);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_world_light_by_index(IntPtr world, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_world_frame_count(IntPtr world);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_world_frame_by_index(IntPtr world, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_world_joint_count(IntPtr world);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_world_joint_by_index(IntPtr world, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_world_physics_count(IntPtr world);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_world_actor_count(IntPtr world);

        // ====================================================================
        // Model
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_name(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_model_static(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_model_self_collide(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_model_allow_auto_disable(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_model_enable_wind(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_model_raw_pose(
            IntPtr model,
            out double px, out double py, out double pz,
            out double rx, out double ry, out double rz, out double rw);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_canonical_link_name(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_pose_relative_to(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_uri(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_model_link_count(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_link_by_index(IntPtr model, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_link_by_name(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_model_joint_count(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_joint_by_index(IntPtr model, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_joint_by_name(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_model_frame_count(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_frame_by_index(IntPtr model, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_model_nested_model_count(IntPtr model);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_nested_model_by_index(IntPtr model, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_model_nested_model_by_name(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        // ====================================================================
        // Link
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_link_name(IntPtr link);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_link_raw_pose(
            IntPtr link,
            out double px, out double py, out double pz,
            out double rx, out double ry, out double rz, out double rw);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_link_pose_relative_to(IntPtr link);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_link_enable_wind(IntPtr link);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_link_enable_gravity(IntPtr link);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_link_kinematic(IntPtr link);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_link_inertial(
            IntPtr link,
            out double mass,
            out double ixx, out double iyy, out double izz,
            out double ixy, out double ixz, out double iyz,
            out double px, out double py, out double pz,
            out double rx, out double ry, out double rz, out double rw);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_link_visual_count(IntPtr link);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_link_visual_by_index(IntPtr link, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_link_visual_by_name(
            IntPtr link,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_link_collision_count(IntPtr link);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_link_collision_by_index(IntPtr link, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_link_collision_by_name(
            IntPtr link,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_link_sensor_count(IntPtr link);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_link_sensor_by_index(IntPtr link, ulong index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_link_sensor_by_name(
            IntPtr link,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_link_light_count(IntPtr link);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_link_light_by_index(IntPtr link, ulong index);

        // ====================================================================
        // Joint
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_joint_name(IntPtr joint);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_joint_type(IntPtr joint);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_joint_parent_name(IntPtr joint);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_joint_child_name(IntPtr joint);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_joint_raw_pose(
            IntPtr joint,
            out double px, out double py, out double pz,
            out double rx, out double ry, out double rz, out double rw);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_joint_pose_relative_to(IntPtr joint);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_thread_pitch(IntPtr joint);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_screw_thread_pitch(IntPtr joint);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong sdf_joint_sensor_count(IntPtr joint);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_joint_sensor_by_index(IntPtr joint, ulong index);

        // ====================================================================
        // JointAxis
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_joint_axis(IntPtr joint, uint index);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_joint_axis_xyz(
            IntPtr axis, out double x, out double y, out double z);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_joint_axis_xyz_expressed_in(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_lower(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_upper(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_effort(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_max_velocity(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_damping(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_friction(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_spring_reference(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_spring_stiffness(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_stiffness(IntPtr axis);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_joint_axis_dissipation(IntPtr axis);

        // ====================================================================
        // Visual
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_visual_name(IntPtr visual);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_visual_cast_shadows(IntPtr visual);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float sdf_visual_transparency(IntPtr visual);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_visual_raw_pose(
            IntPtr visual,
            out double px, out double py, out double pz,
            out double rx, out double ry, out double rz, out double rw);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_visual_pose_relative_to(IntPtr visual);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_visual_geometry(IntPtr visual);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_visual_material(IntPtr visual);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint sdf_visual_visibility_flags(IntPtr visual);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_visual_has_laser_retro(IntPtr visual);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_visual_laser_retro(IntPtr visual);

        // ====================================================================
        // Collision
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_collision_name(IntPtr collision);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_collision_raw_pose(
            IntPtr collision,
            out double px, out double py, out double pz,
            out double rx, out double ry, out double rz, out double rw);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_collision_geometry(IntPtr collision);

        // ====================================================================
        // Geometry
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_geometry_type(IntPtr geom);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_geometry_box_size(
            IntPtr geom, out double x, out double y, out double z);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_geometry_sphere_radius(IntPtr geom);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_geometry_cylinder(
            IntPtr geom, out double radius, out double length);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_geometry_capsule(
            IntPtr geom, out double radius, out double length);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_geometry_cone(
            IntPtr geom, out double radius, out double length);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_geometry_ellipsoid_radii(
            IntPtr geom, out double x, out double y, out double z);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_geometry_plane(
            IntPtr geom,
            out double nx, out double ny, out double nz,
            out double sx, out double sy);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_geometry_mesh_uri(IntPtr geom);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_geometry_mesh_file_path(IntPtr geom);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_geometry_mesh_scale(
            IntPtr geom, out double x, out double y, out double z);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_geometry_mesh_submesh(IntPtr geom);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_geometry_mesh_center_submesh(IntPtr geom);

        // ====================================================================
        // Material
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_material_ambient(
            IntPtr mat, out float r, out float g, out float b, out float a);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_material_diffuse(
            IntPtr mat, out float r, out float g, out float b, out float a);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_material_specular(
            IntPtr mat, out float r, out float g, out float b, out float a);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_material_emissive(
            IntPtr mat, out float r, out float g, out float b, out float a);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_material_shininess(IntPtr mat);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_material_render_order(IntPtr mat);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_material_lighting(IntPtr mat);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_material_double_sided(IntPtr mat);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_material_script_uri(IntPtr mat);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_material_script_name(IntPtr mat);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_material_shader_type(IntPtr mat);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_material_normal_map(IntPtr mat);

        // ====================================================================
        // Sensor
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_sensor_name(IntPtr sensor);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_sensor_type(IntPtr sensor);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_sensor_type_string(IntPtr sensor);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_sensor_update_rate(IntPtr sensor);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_sensor_topic(IntPtr sensor);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_sensor_raw_pose(
            IntPtr sensor,
            out double px, out double py, out double pz,
            out double rx, out double ry, out double rz, out double rw);

        // ====================================================================
        // Light
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_light_name(IntPtr light);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_light_type(IntPtr light);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_light_raw_pose(
            IntPtr light,
            out double px, out double py, out double pz,
            out double rx, out double ry, out double rz, out double rw);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_light_diffuse(
            IntPtr light, out float r, out float g, out float b, out float a);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_light_specular(
            IntPtr light, out float r, out float g, out float b, out float a);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_light_intensity(IntPtr light);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_light_range(IntPtr light);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_light_linear_attenuation(IntPtr light);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_light_constant_attenuation(IntPtr light);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double sdf_light_quadratic_attenuation(IntPtr light);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int sdf_light_cast_shadows(IntPtr light);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_light_direction(
            IntPtr light, out double x, out double y, out double z);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_light_spot(
            IntPtr light,
            out double innerAngle, out double outerAngle, out double falloff);

        // ====================================================================
        // Frame
        // ====================================================================
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_frame_name(IntPtr frame);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_frame_attached_to(IntPtr frame);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sdf_frame_raw_pose(
            IntPtr frame,
            out double px, out double py, out double pz,
            out double rx, out double ry, out double rz, out double rw);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr sdf_frame_pose_relative_to(IntPtr frame);
    }
}

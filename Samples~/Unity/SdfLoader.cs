// Example Unity MonoBehaviour that loads an SDF world file and
// spawns GameObjects for each model/link/visual.
//
// Usage:
//   1. Attach this script to an empty GameObject in your scene.
//   2. Set the "Sdf File Path" field to an .sdf or .world file.
//   3. Press Play — the SDF scene hierarchy is created as GameObjects.

using UnityEngine;
using SdFormat;

public class SdfLoader : MonoBehaviour
{
    [Header("SDF File")]
    [Tooltip("Path to the .sdf or .world file to load")]
    public string sdfFilePath = "";

    [Header("Options")]
    [Tooltip("Scale factor (SDF uses meters, Unity uses meters by default)")]
    public float scaleFactor = 1.0f;

    void Start()
    {
        if (string.IsNullOrEmpty(sdfFilePath))
        {
            Debug.LogWarning("[SdfLoader] No SDF file path specified.");
            return;
        }

        LoadSdf(sdfFilePath);
    }

    public void LoadSdf(string path)
    {
        using var root = new SdfRoot();

        try
        {
            root.LoadFile(path);
        }
        catch (SdfException ex)
        {
            Debug.LogError($"[SdfLoader] Failed to load SDF: {ex.Message}");
            return;
        }

        Debug.Log($"[SdfLoader] Loaded SDF version {root.Version}");

        // Load worlds
        for (ulong w = 0; w < root.WorldCount; w++)
        {
            var world = root.WorldByIndex(w);
            if (world == null) continue;

            var worldGo = new GameObject($"World_{world.Name}");
            worldGo.transform.SetParent(transform);

            Debug.Log($"[SdfLoader] World: {world.Name}, " +
                      $"Gravity: {world.Gravity}, " +
                      $"Models: {world.ModelCount}");

            // Set Unity physics gravity from SDF
            // SDF uses Z-up, Unity uses Y-up — convert coordinate frame
            var g = world.Gravity;
            Physics.gravity = SdfToUnityVector3(g) * scaleFactor;

            for (ulong m = 0; m < world.ModelCount; m++)
            {
                var model = world.ModelByIndex(m);
                if (model == null) continue;
                CreateModel(model, worldGo.transform);
            }

            // Lights
            for (ulong l = 0; l < world.LightCount; l++)
            {
                var light = world.LightByIndex(l);
                if (light == null) continue;
                CreateLight(light, worldGo.transform);
            }
        }

        // Handle model-only SDF files (no world)
        if (root.WorldCount == 0 && root.Model != null)
        {
            CreateModel(root.Model, transform);
        }
    }

    void CreateModel(SdfModel model, Transform parent)
    {
        var modelGo = new GameObject($"Model_{model.Name}");
        modelGo.transform.SetParent(parent);
        ApplySdfPose(modelGo.transform, model.RawPose);

        if (model.IsStatic)
        {
            modelGo.isStatic = true;
        }

        Debug.Log($"[SdfLoader]   Model: {model.Name} " +
                  $"(links={model.LinkCount}, joints={model.JointCount})");

        // Links
        for (ulong i = 0; i < model.LinkCount; i++)
        {
            var link = model.LinkByIndex(i);
            if (link == null) continue;
            CreateLink(link, modelGo.transform, model.IsStatic);
        }

        // Joints
        for (ulong i = 0; i < model.JointCount; i++)
        {
            var joint = model.JointByIndex(i);
            if (joint == null) continue;
            LogJoint(joint);
        }

        // Nested models
        for (ulong i = 0; i < model.NestedModelCount; i++)
        {
            var nested = model.NestedModelByIndex(i);
            if (nested == null) continue;
            CreateModel(nested, modelGo.transform);
        }
    }

    void CreateLink(SdfLink link, Transform parent, bool isStatic)
    {
        var linkGo = new GameObject($"Link_{link.Name}");
        linkGo.transform.SetParent(parent);
        ApplySdfPose(linkGo.transform, link.RawPose);

        // Add Rigidbody for non-static links with mass > 0
        var inertial = link.Inertial;
        if (!isStatic && inertial.Mass > 0)
        {
            var rb = linkGo.AddComponent<Rigidbody>();
            rb.mass = (float)inertial.Mass;
            rb.useGravity = link.EnableGravity;
            rb.isKinematic = link.Kinematic;
        }

        // Visuals
        for (ulong i = 0; i < link.VisualCount; i++)
        {
            var visual = link.VisualByIndex(i);
            if (visual == null) continue;
            CreateVisual(visual, linkGo.transform);
        }

        // Collisions
        for (ulong i = 0; i < link.CollisionCount; i++)
        {
            var collision = link.CollisionByIndex(i);
            if (collision == null) continue;
            CreateCollision(collision, linkGo.transform);
        }
    }

    void CreateVisual(SdfVisual visual, Transform parent)
    {
        var geom = visual.Geometry;
        if (geom == null) return;

        GameObject visualGo = CreatePrimitiveFromGeometry(geom, $"Visual_{visual.Name}");
        if (visualGo == null) return;

        visualGo.transform.SetParent(parent);
        ApplySdfPose(visualGo.transform, visual.RawPose);

        // Apply material color
        var mat = visual.Material;
        if (mat != null)
        {
            var renderer = visualGo.GetComponent<Renderer>();
            if (renderer != null)
            {
                var diffuse = mat.Diffuse;
                renderer.material.color = new Color(diffuse.R, diffuse.G, diffuse.B, diffuse.A);

                if (visual.Transparency > 0)
                {
                    var color = renderer.material.color;
                    color.a = 1.0f - visual.Transparency;
                    renderer.material.color = color;
                }
            }
        }

        // Log mesh URI for mesh geometry (needs custom mesh loading)
        if (geom.Type == GeometryType.Mesh)
        {
            Debug.Log($"[SdfLoader]     Mesh: {geom.MeshUri} " +
                      $"(scale: {geom.MeshScale})");
        }
    }

    void CreateCollision(SdfCollision collision, Transform parent)
    {
        var geom = collision.Geometry;
        if (geom == null) return;

        var collGo = new GameObject($"Collision_{collision.Name}");
        collGo.transform.SetParent(parent);
        ApplySdfPose(collGo.transform, collision.RawPose);

        switch (geom.Type)
        {
            case GeometryType.Box:
                var box = collGo.AddComponent<BoxCollider>();
                var bs = geom.BoxSize;
                // SDF XYZ -> Unity XZY (coordinate swap)
                box.size = new Vector3((float)bs.X, (float)bs.Z, (float)bs.Y) * scaleFactor;
                break;

            case GeometryType.Sphere:
                var sphere = collGo.AddComponent<SphereCollider>();
                sphere.radius = (float)geom.SphereRadius * scaleFactor;
                break;

            case GeometryType.Cylinder:
                // Unity doesn't have a native cylinder collider — use capsule as approximation
                var capsule = collGo.AddComponent<CapsuleCollider>();
                capsule.radius = (float)geom.CylinderRadius * scaleFactor;
                capsule.height = (float)geom.CylinderLength * scaleFactor;
                break;

            case GeometryType.Capsule:
                var cap = collGo.AddComponent<CapsuleCollider>();
                cap.radius = (float)geom.CapsuleRadius * scaleFactor;
                cap.height = (float)geom.CapsuleLength * scaleFactor;
                break;

            case GeometryType.Mesh:
                // Mesh colliders need actual mesh data — log for now
                var mc = collGo.AddComponent<MeshCollider>();
                mc.convex = true;
                Debug.Log($"[SdfLoader]     Collision mesh: {geom.MeshUri} " +
                          $"(needs mesh loading implementation)");
                break;

            case GeometryType.Plane:
                // Infinite plane — approximate with large box
                var planeBox = collGo.AddComponent<BoxCollider>();
                planeBox.size = new Vector3(1000, 0.01f, 1000);
                break;

            default:
                Debug.LogWarning($"[SdfLoader] Unsupported collision geometry: {geom.Type}");
                break;
        }
    }

    void CreateLight(SdfLight light, Transform parent)
    {
        var lightGo = new GameObject($"Light_{light.Name}");
        lightGo.transform.SetParent(parent);
        ApplySdfPose(lightGo.transform, light.RawPose);

        var unityLight = lightGo.AddComponent<Light>();
        var diffuse = light.Diffuse;
        unityLight.color = new Color(diffuse.R, diffuse.G, diffuse.B, diffuse.A);
        unityLight.intensity = (float)light.Intensity;
        unityLight.range = (float)light.Range * scaleFactor;
        unityLight.shadows = light.CastShadows ? LightShadows.Soft : LightShadows.None;

        switch (light.Type)
        {
            case LightType.Point:
                unityLight.type = UnityEngine.LightType.Point;
                break;
            case LightType.Directional:
                unityLight.type = UnityEngine.LightType.Directional;
                break;
            case LightType.Spot:
                unityLight.type = UnityEngine.LightType.Spot;
                var spot = light.Spot;
                unityLight.spotAngle = (float)spot.OuterAngle * Mathf.Rad2Deg * 2f;
                unityLight.innerSpotAngle = (float)spot.InnerAngle * Mathf.Rad2Deg * 2f;
                break;
        }

        Debug.Log($"[SdfLoader]   Light: {light.Name} ({light.Type})");
    }

    void LogJoint(SdfJoint joint)
    {
        Debug.Log($"[SdfLoader]   Joint: {joint.Name} ({joint.Type}) " +
                  $"{joint.ParentName} -> {joint.ChildName}");

        var axis = joint.Axis(0);
        if (axis != null)
        {
            Debug.Log($"[SdfLoader]     Axis: {axis.Xyz}, " +
                      $"Limits: [{axis.Lower:F3}, {axis.Upper:F3}], " +
                      $"Effort: {axis.Effort:F1}, MaxVel: {axis.MaxVelocity:F1}");
        }
    }

    // ========================================================================
    // Geometry → Unity primitive helpers
    // ========================================================================

    GameObject CreatePrimitiveFromGeometry(SdfGeometry geom, string name)
    {
        GameObject go = null;

        switch (geom.Type)
        {
            case GeometryType.Box:
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.name = name;
                var bs = geom.BoxSize;
                go.transform.localScale = new Vector3(
                    (float)bs.X, (float)bs.Z, (float)bs.Y) * scaleFactor;
                break;

            case GeometryType.Sphere:
                go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.name = name;
                float d = (float)geom.SphereRadius * 2f * scaleFactor;
                go.transform.localScale = new Vector3(d, d, d);
                break;

            case GeometryType.Cylinder:
                go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                go.name = name;
                float cr = (float)geom.CylinderRadius * 2f * scaleFactor;
                float cl = (float)geom.CylinderLength * 0.5f * scaleFactor;
                go.transform.localScale = new Vector3(cr, cl, cr);
                break;

            case GeometryType.Plane:
                go = GameObject.CreatePrimitive(PrimitiveType.Plane);
                go.name = name;
                break;

            case GeometryType.Mesh:
                // Placeholder — create an empty object; real mesh loading is project-specific
                go = new GameObject(name);
                Debug.Log($"[SdfLoader]     TODO: Load mesh from {geom.MeshUri}");
                break;

            default:
                Debug.LogWarning($"[SdfLoader] Unsupported visual geometry: {geom.Type}");
                break;
        }

        return go;
    }

    // ========================================================================
    // Coordinate frame conversion: SDF (Z-up, RHS) → Unity (Y-up, LHS)
    // ========================================================================

    /// <summary>
    /// Convert SDF Z-up right-hand vector to Unity Y-up left-hand vector.
    /// SDF (X, Y, Z) → Unity (X, Z, Y) with negated X for handedness.
    /// </summary>
    static Vector3 SdfToUnityVector3(SdfVector3d v)
    {
        // SDF: X-forward, Y-left, Z-up (right-handed)
        // Unity: X-right, Y-up, Z-forward (left-handed)
        return new Vector3(-(float)v.X, (float)v.Z, (float)v.Y);
    }

    /// <summary>
    /// Convert SDF pose to Unity transform.
    /// </summary>
    void ApplySdfPose(Transform t, SdfPose3d pose)
    {
        t.localPosition = SdfToUnityVector3(pose.Position) * scaleFactor;

        // Convert quaternion from SDF (Z-up RHS) to Unity (Y-up LHS)
        var q = pose.Rotation;
        t.localRotation = new Quaternion(
            (float)q.X,
            (float)q.Z,    // SDF Y → Unity Z
            (float)q.Y,    // SDF Z → Unity Y
            -(float)q.W    // negate W for handedness flip
        );
    }
}

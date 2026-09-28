using UnityEngine;

/// <summary>
/// Planar mirror that renders a reflected view of playerCamera into a RenderTexture using a reflection camera.
/// Includes oblique clip-plane to avoid near-plane artifacts.
/// Designed for XR: by default we use a mono RenderTexture (MirrorCamera.stereoTargetEye = None).
/// </summary>
[RequireComponent(typeof(Renderer))]
public class PlanarMirrorXR : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;         // assign your XR center eye/main camera
    public Camera reflectionCamera;     // the camera that renders to the RenderTexture
    public RenderTexture targetTexture; // the RenderTexture used by mirror material
    public LayerMask reflectionLayers = 0; // which layers to render (use AvatarMirror)

    [Header("Performance")]
    [Tooltip("If >1, the mirror updates every N frames (useful on Quest).")]
    public int updateEveryNFrames = 1; // 1 = every frame, 2 = every 2 frames, etc.

    private int frameCounter = 0;
    private Renderer rend;
    private Material mat;

    void Start()
    {
        if (!playerCamera || !reflectionCamera || !targetTexture)
        {
            Debug.LogError("PlanarMirrorXR: assign playerCamera, reflectionCamera and targetTexture.");
            enabled = false;
            return;
        }

        rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.mainTexture = targetTexture;

        reflectionCamera.targetTexture = targetTexture;
        reflectionCamera.cullingMask = reflectionLayers;
        reflectionCamera.enabled = false; // manual rendering
        // Ensure mono render into RT for XR:
        reflectionCamera.stereoTargetEye = StereoTargetEyeMask.None;
    }

    void LateUpdate()
    {
        frameCounter++;
        if (updateEveryNFrames > 1 && frameCounter % updateEveryNFrames != 0) return;

        UpdateReflectionCamera();
        reflectionCamera.Render();
    }

    void UpdateReflectionCamera()
    {
        // Mirror plane in world space. mirrorNormal points away from front face (assumes forward faces viewer).
        Vector3 planePos = transform.position;
        Vector3 planeNormal = transform.forward.normalized; // front side of the mirror

        // Reflect camera position across plane
        float d = Vector3.Dot(planeNormal, playerCamera.transform.position - planePos);
        Vector3 reflectedPos = playerCamera.transform.position - 2f * d * planeNormal;

        // Reflect camera orientation
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camUp = playerCamera.transform.up;
        Vector3 reflectedForward = Vector3.Reflect(camForward, planeNormal);
        Vector3 reflectedUp = Vector3.Reflect(camUp, planeNormal);

        reflectionCamera.transform.position = reflectedPos;
        reflectionCamera.transform.rotation = Quaternion.LookRotation(reflectedForward, reflectedUp);

        // Match projection (FOV, aspect, near/far)
        reflectionCamera.fieldOfView = playerCamera.fieldOfView;
        reflectionCamera.aspect = playerCamera.aspect;
        reflectionCamera.nearClipPlane = playerCamera.nearClipPlane;
        reflectionCamera.farClipPlane = playerCamera.farClipPlane;

        // Apply oblique clipping so geometry behind the mirror is clipped properly
        // Construct plane in reflection camera space
        Vector4 clipPlane = CameraSpacePlane(reflectionCamera, planePos, planeNormal, 1.0f);
        Matrix4x4 projection = playerCamera.projectionMatrix; // start with player's projection
        projection = CalculateObliqueMatrix(projection, clipPlane);
        reflectionCamera.projectionMatrix = projection;
    }

    // Returns plane in camera space
    Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
    {
        Vector3 offsetPos = pos + normal * 0.01f; // small offset to avoid z-fighting
        Matrix4x4 m = cam.worldToCameraMatrix;
        Vector3 cpos = m.MultiplyPoint(offsetPos);
        Vector3 cnormal = m.MultiplyVector(normal).normalized * sideSign;
        return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
    }

    // Adjust projection matrix so that near clipping plane becomes the supplied plane
    Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
    {
        Vector4 q = projection.inverse * new Vector4(
            Sign(clipPlane.x),
            Sign(clipPlane.y),
            1.0f,
            1.0f
        );
        Vector4 c = clipPlane * (2.0F / Vector4.Dot(clipPlane, q));
        // third row = clip plane - fourth row
        projection[2] = c.x - projection[3];
        projection[6] = c.y - projection[7];
        projection[10] = c.z - projection[11];
        projection[14] = c.w - projection[15];
        return projection;
    }

    float Sign(float a) { return a >= 0.0f ? 1.0f : -1.0f; }
}

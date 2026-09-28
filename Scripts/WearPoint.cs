using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WearPoint : MonoBehaviour
{
    public PPEType type;                // What can be worn here
    public Vector3 localPositionOffset; // fine-tune if needed
    public Vector3 localRotationOffset; // degrees

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}

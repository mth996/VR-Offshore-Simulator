using UnityEngine;

public class FullBodyTextureSwapAuto : MonoBehaviour
{
    [Header("Just assign ALL replacement textures in order")]
    public Texture[] jacketTextures;

    private SkinnedMeshRenderer[] skinnedRenderers;
    private bool applied = false;

    void OnEnable()
    {
        PPEManager.OnPPEChanged += OnPPEUpdated;
    }

    void OnDisable()
    {
        PPEManager.OnPPEChanged -= OnPPEUpdated;
    }

    void Start()
    {
        // Auto-detect ALL SkinnedMeshRenderers in the character
        skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);
    }

    void OnPPEUpdated(PPEType type, bool isWorn)
    {
        if (applied) return;
        if (type != PPEType.Jacket) return;
        if (!isWorn) return;

        ApplyTextures();
        applied = true;
    }

    void ApplyTextures()
    {
        int texIndex = 0;

        foreach (var rend in skinnedRenderers)
        {
            Material[] mats = rend.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                if (texIndex >= jacketTextures.Length)
                    return; // no more replacement textures

                Texture t = jacketTextures[texIndex];

                if (mats[i].HasProperty("_BaseMap"))
                    mats[i].SetTexture("_BaseMap", t);
                else
                    mats[i].SetTexture("_MainTex", t);

                texIndex++;
            }

            rend.materials = mats;
        }
    }
}

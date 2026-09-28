using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PPEUIController : MonoBehaviour
{
    [Header("Status Texts")]
    public TextMeshProUGUI hardHatStatus;
    public TextMeshProUGUI glassesStatus;
    public TextMeshProUGUI faceShieldStatus;
    public TextMeshProUGUI earMuffsStatus;
    public TextMeshProUGUI glovesStatus;
    public TextMeshProUGUI jacketStatus;
    public TextMeshProUGUI bootsStatus;

    [Header("Equipped Icons")]
    public Image iconHead;
    public Image iconEyes;
    public Image iconEars;
    public Image iconHands;
    public Image iconBody;
    public Image iconFeet;

    [Header("Proceed Button")]
    public Button proceedButton;

    Color okColor = new Color(0.2f, 0.8f, 0.3f);
    Color badColor = new Color(0.9f, 0.3f, 0.3f);
    Color neutral = Color.gray;

    void OnEnable()
    {
        PPEManager.OnPPEChanged += OnPPEChanged;
        RefreshAll();
    }

    void OnDisable()
    {
        PPEManager.OnPPEChanged -= OnPPEChanged;
    }

    void OnPPEChanged(PPEType type, bool worn)
    {
        SetStatus(type, worn);
        SetIcon(type, worn);
        UpdateProceedButton();
    }

    void RefreshAll()
    {
        foreach (PPEType t in System.Enum.GetValues(typeof(PPEType)))
        {
            bool worn = PPEManager.Instance != null && PPEManager.Instance.IsPPEWorn(t);
            SetStatus(t, worn);
            SetIcon(t, worn);
        }
        UpdateProceedButton();
    }

    void SetStatus(PPEType type, bool worn)
    {
        TextMeshProUGUI target = null;
        bool required = false;

        var mgr = PPEManager.Instance;

        switch (type)
        {
            case PPEType.HardHat:
                target = hardHatStatus;
                required = mgr != null && mgr.requireHardHat;
                break;
            case PPEType.Glasses:
                target = glassesStatus;
                required = mgr != null && mgr.requireGlasses;
                break;
            case PPEType.FaceShield:
                target = faceShieldStatus;
                required = mgr != null && mgr.requireFaceShield;
                break;
            case PPEType.EarMuffs:
                target = earMuffsStatus;
                required = mgr != null && mgr.requireEarMuffs;
                break;
            case PPEType.Gloves:
                target = glovesStatus;
                required = mgr != null && mgr.requireGloves;
                break;
            case PPEType.Jacket:
                target = jacketStatus;
                required = mgr != null && mgr.requireJacket;
                break;
            case PPEType.Boots:
                target = bootsStatus;
                required = mgr != null && mgr.requireBoots;
                break;
        }

        if (target == null) return;

        if (!required)
        {
            target.text = "—";
            target.color = neutral;
        }
        else if (worn)
        {
            target.text = "✓";
            target.color = okColor;
        }
        else
        {
            target.text = "✖";
            target.color = badColor;
        }
    }

    void SetIcon(PPEType type, bool on)
    {
        Image img = null;
        switch (type)
        {
            case PPEType.HardHat: img = iconHead; break;
            case PPEType.Glasses: img = iconEyes; break;
            case PPEType.EarMuffs: img = iconEars; break;
            case PPEType.Gloves: img = iconHands; break;
            case PPEType.Jacket: img = iconBody; break;
            case PPEType.Boots: img = iconFeet; break;
        }

        if (img == null) return;

        var c = img.color;
        c.a = on ? 1f : 0.25f;
        img.color = c;
    }

    void UpdateProceedButton()
    {
        if (proceedButton == null || PPEManager.Instance == null) return;
        proceedButton.interactable = PPEManager.Instance.AreAllRequiredWorn();
    }

    // Hook this to the Proceed button OnClick
    public void OnProceedClicked()
    {
        Debug.Log("All required PPE worn – go to next scene!");
        // e.g. SceneManager.LoadScene("Fire_Explosion_Scene");
    }
}

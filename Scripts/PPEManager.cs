using System;
using System.Collections.Generic;
using UnityEngine;

public class PPEManager : MonoBehaviour
{
    public static PPEManager Instance { get; private set; }

    // Fired whenever an item is worn / unworn
    public static event Action<PPEType, bool> OnPPEChanged;

    // Which items are REQUIRED for this scenario
    [Header("Required PPE for this module")]
    public bool requireHardHat = true;
    public bool requireGlasses = true;
    public bool requireFaceShield = true;
    public bool requireEarMuffs = true;
    public bool requireGloves = true;
    public bool requireJacket = true;
    public bool requireBoots = true;

    Dictionary<PPEType, bool> worn = new Dictionary<PPEType, bool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // init all to false
        foreach (PPEType type in Enum.GetValues(typeof(PPEType)))
        {
            worn[type] = false;
        }
    }

    // Called by PPEItemWearable
    public void SetPPEWorn(PPEType type, bool isWorn)
    {
        worn[type] = isWorn;
        OnPPEChanged?.Invoke(type, isWorn);
    }

    public bool IsPPEWorn(PPEType type) => worn.ContainsKey(type) && worn[type];

    public bool AreAllRequiredWorn()
    {
        bool ok = true;

        if (requireHardHat) ok &= IsPPEWorn(PPEType.HardHat);
        if (requireGlasses) ok &= IsPPEWorn(PPEType.Glasses);
        if (requireFaceShield) ok &= IsPPEWorn(PPEType.FaceShield);
        if (requireEarMuffs) ok &= IsPPEWorn(PPEType.EarMuffs);
        if (requireGloves) ok &= IsPPEWorn(PPEType.Gloves);
        if (requireJacket) ok &= IsPPEWorn(PPEType.Jacket);
        if (requireBoots) ok &= IsPPEWorn(PPEType.Boots);

        return ok;
    }
}

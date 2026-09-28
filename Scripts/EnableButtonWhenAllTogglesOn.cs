using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnableButtonWhenAllTogglesOn : MonoBehaviour
{
    [Header("Toggles To Check")]
    public List<Toggle> toggles = new List<Toggle>();

    [Header("Target Button")]
    public Button targetButton;

    void Start()
    {
        UpdateButtonState();

        // Listen for toggle changes
        foreach (var t in toggles)
        {
            if (t != null)
                t.onValueChanged.AddListener(delegate { UpdateButtonState(); });
        }
    }

    void UpdateButtonState()
    {
        if (targetButton == null) return;

        foreach (var t in toggles)
        {
            if (t == null || !t.isOn)
            {
                targetButton.interactable = false;
                return;
            }
        }

        // If we reach here → all toggles ON
        targetButton.interactable = true;
    }
}
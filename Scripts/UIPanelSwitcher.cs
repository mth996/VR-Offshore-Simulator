using System;
using UnityEngine;

public class UIPanelSwitcher : MonoBehaviour
{
    public GameObject introPanel;
    public GameObject checklistPanel;
    public GameObject proceedPanel;
    public GameObject DoorTrigger;

    private void Awake()
    {
        introPanel.SetActive(true);
        checklistPanel.SetActive(false);
        proceedPanel.SetActive(false);
        DoorTrigger.SetActive(false);
    }

    // Call this from button
    public void ShowChecklist()
    {
        introPanel.SetActive(false);
        checklistPanel.SetActive(true);
        proceedPanel.SetActive(false);
        DoorTrigger.SetActive(false);
    }

    public void ShowProceedPanel()
    {
        introPanel.SetActive(false);
        checklistPanel.SetActive(false);
        proceedPanel.SetActive(true);
        DoorTrigger.SetActive(true);
    }

    // Optional: go back
    public void ShowIntro()
    {
        checklistPanel.SetActive(false);
        introPanel.SetActive(true);
    }
}

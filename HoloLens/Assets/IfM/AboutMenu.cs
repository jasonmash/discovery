using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Assign DialogMedium_192x128.prefab")]
    private GameObject dialogPrefabMedium;

    /// <summary>
    /// Medium Dialog example prefab to display
    /// </summary>
    public GameObject DialogPrefabMedium
    {
        get => dialogPrefabMedium;
        set => dialogPrefabMedium = value;
    }

    public bool isOpen;

    private void Awake()
    {
    }

    public void Click()
    {
        if (isOpen) { return; }
        Dialog myDialog = Dialog.Open(dialogPrefabMedium, DialogButtonType.Close, "About", "This is a demonstration app for the University of Cambridge dissertation project, being conducted as part of the ISMM course at the Institute for Manufacturing.", true);
        if (myDialog != null)
        {
            isOpen = true;
            myDialog.OnClosed += OnClosedDialogEvent;
        }
    }

    private void OnClosedDialogEvent(DialogResult obj)
    {
        isOpen = false;
    }
}

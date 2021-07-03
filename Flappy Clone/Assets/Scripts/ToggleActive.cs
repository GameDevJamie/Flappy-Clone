using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functions for setting a gameobject active status
/// </summary>
public class ToggleActive : MonoBehaviour
{
    [SerializeField]
    private bool Active = true;

    private void Awake()
    {
        if (Active) Show();
        else Hide();
    }

    public void Show()
    {
        Active = true;
        this.gameObject.SetActive(Active);
    }

    public void Hide()
    {
        Active = false;
        this.gameObject.SetActive(Active);
    }
}

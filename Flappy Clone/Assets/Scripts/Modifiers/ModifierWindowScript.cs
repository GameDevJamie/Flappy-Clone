using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierWindowScript : MonoBehaviour
{
    private Toggle m_WeightMod;
    private Toggle m_ShiftingPipesMod;
    private Toggle m_MirrorMod;

    // Start is called before the first frame update
    private void Start()
    {
        m_WeightMod = this.transform.Find("WeightMod").GetComponent<Toggle>();
        m_ShiftingPipesMod = this.transform.Find("ShiftingPipesMod").GetComponent<Toggle>();
        m_MirrorMod = this.transform.Find("MirrorMod").GetComponent<Toggle>();

        Hide();
    }


    /// <summary>
    /// Spawns all Modifier gameobjects selected
    /// </summary>
    public void SpawnModifiers()
    {
        if (m_WeightMod.isOn)
        {
            GameObject g = new GameObject("Modifier: Weight", typeof(WeightMod));
            g.tag = "Modifier";
        }
        if(m_ShiftingPipesMod.isOn)
        {
            //Spawn Mod
        }
        if(m_MirrorMod.isOn)
        {
            GameObject g = new GameObject("Modifier: Mirror", typeof(MirrorMod));
            g.tag = "Modifier";
        }
    }

    /// <summary>
    /// Destroys all active Modifiers
    /// </summary>
    public void DestroyModifiers()
    {
        foreach(GameObject mod in GameObject.FindGameObjectsWithTag("Modifier"))
        {
            Destroy(mod);
        }
    }

    #region Public Methods
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public bool IsOpen()
    {
        return this.gameObject.activeSelf;
    }
    #endregion
}

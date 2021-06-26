using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierWindowScript : MonoBehaviour
{
    private bool m_WeightMod;
    private bool m_ShiftingPipesMod;
    private bool m_MirrorMod;

    private void Awake()
    {
        m_WeightMod = false;
        m_ShiftingPipesMod = false;
        m_MirrorMod = false;
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.transform.Find("WeightModBtn").GetComponent<Button>().onClick.AddListener(WeightModClick);
        this.transform.Find("ShiftingPipesModBtn").GetComponent<Button>().onClick.AddListener(ShiftingPipesModClick);
        this.transform.Find("MirrorModBtn").GetComponent<Button>().onClick.AddListener(MirrorModClick);
    }

    private void WeightModClick()
    {
        m_WeightMod = !m_WeightMod;
        Debug.Log("Weight Mod: " + m_WeightMod);
    }

    private void ShiftingPipesModClick()
    {
        m_ShiftingPipesMod = !m_ShiftingPipesMod;
        Debug.Log("Shifting Pipes Mod: " + m_ShiftingPipesMod);
    }

    private void MirrorModClick()
    {
        m_MirrorMod = !m_MirrorMod;
        Debug.Log("Mirror Mod: " + m_MirrorMod);
    }


    /// <summary>
    /// Spawns all Modifier gameobjects selected
    /// </summary>
    public void SpawnModifiers()
    {
        if (m_WeightMod)
        {
            GameObject g = new GameObject("Modifier: Weight", typeof(WeightMod));
            g.tag = "Modifier";
        }
        if(m_ShiftingPipesMod)
        {
            //Spawn Mod
        }
        if(m_MirrorMod)
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
    #endregion
}

using UnityEngine;
using UnityEngine.UI;

public class SettingsWindowScript : MonoBehaviour
{
    private struct SMod
    {
        public EModType Type;
        public bool IsOn;
    }
    private SMod[] m_ModList;

    private void Awake()
    {
        m_ModList = new SMod[ModManager.GetNumMods()];
        m_ModList[0] = new SMod { Type = EModType.WEIGHT,   IsOn = false };
        m_ModList[1] = new SMod { Type = EModType.SHIFT,    IsOn = false };
        m_ModList[2] = new SMod { Type = EModType.MIRROR,   IsOn = false };
        //m_ModList[3] = new SMod { Type = EModType.SPEED,    IsOn = false };

        Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Check which Mods are active]
        for(int i = 0; i < m_ModList.Length; ++i)
        {
            m_ModList[i].IsOn = ModManager.IsModActive(m_ModList[i].Type);
        }

        transform.Find("WeightModToggle").GetComponent<Toggle>().isOn = m_ModList[(int)EModType.WEIGHT].IsOn;
        transform.Find("WeightModToggle").GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleMod(EModType.WEIGHT); });

        transform.Find("ShiftPipesModToggle").GetComponent<Toggle>().isOn = m_ModList[(int)EModType.SHIFT].IsOn;
        transform.Find("ShiftPipesModToggle").GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleMod(EModType.SHIFT); });

        transform.Find("MirrorModToggle").GetComponent<Toggle>().isOn = m_ModList[(int)EModType.MIRROR].IsOn;
        transform.Find("MirrorModToggle").GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleMod(EModType.MIRROR); });
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void ToggleShow()
    {
        if (this.gameObject.activeSelf) Hide();
        else Show();
    }


    private void ToggleMod(EModType type)
    {
        m_ModList[(int)type].IsOn = !m_ModList[(int)type].IsOn;

        Debug.Log("Toggle Called");
        Debug.Log("Toggle: " + m_ModList[(int)type].IsOn);
    }

    public void SaveModSelection()
    {
        for (int i = 0; i < m_ModList.Length; ++i)
        {
            ModManager.SetModActive(m_ModList[i].Type, m_ModList[i].IsOn);
        }
    }
}

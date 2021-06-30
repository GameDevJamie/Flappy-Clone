using UnityEngine;
using UnityEngine.UI;

public class ToggleColour : MonoBehaviour
{
    [SerializeField]
    private Color OffColorNormal;
    [SerializeField]
    private Color OffColorHighlight;
    [SerializeField]
    private Color OffColorPressed;
    [SerializeField]
    private Color OffColorSelected;

    [SerializeField]
    private Color OnColorNormal;
    [SerializeField]
    private Color OnColorHighlight;
    [SerializeField]
    private Color OnColorPressed;
    [SerializeField]
    private Color OnColorSelected;

    private Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        OnToggleValueChanged(toggle.isOn);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        ColorBlock cb = toggle.colors;
        if (isOn)
        {
            cb.normalColor = OnColorNormal;
            cb.highlightedColor = OnColorHighlight;
            cb.pressedColor = OnColorPressed;
            cb.selectedColor = OnColorSelected;
        }
        else
        {
            cb.normalColor = OffColorNormal;
            cb.highlightedColor = OffColorHighlight;
            cb.pressedColor = OffColorPressed;
            cb.selectedColor = OffColorSelected;
        }

        toggle.colors = cb;
    }
}

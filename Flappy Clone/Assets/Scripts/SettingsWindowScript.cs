using UnityEngine;
using UnityEngine.UI;

public class SettingsWindowScript : MonoBehaviour
{
    [SerializeField]
    private GameObject VolumeSlider;
    [SerializeField]
    private GameObject GravitySlider;
    [SerializeField]
    private GameObject SpeedSlider;
    [SerializeField]
    private GameObject ShiftSlider;
    [SerializeField]
    private GameObject MirrorSlider;

    private void Awake()
    {
        GameSettings.SetGameSpeed(GameSettings.GetGameSpeed());
        //GameSettings.SetGravityStrength(GameSettings.GetGravityStrength());
        GameSettings.SetMasterVolume(GameSettings.GetMasterVolume());
        //GameSettings.SetMirrorMode(GameSettings.GetMirrorMode());
        //GameSettings.SetShiftPipesMode(GameSettings.GetShiftPipesMode());

        gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdateGravityText(GameSettings.GetGravityStrength());
        UpdateGameSpeedText(GameSettings.GetGameSpeed());
        UpdateShiftPipesModeText(GameSettings.GetShiftPipesMode());
        UpdateMirrorModeText(GameSettings.GetMirrorMode());

        //Add Listeners to Sliders
        var gravSlider = GravitySlider.GetComponentInChildren<Slider>();
        gravSlider.onValueChanged.AddListener(delegate { SetGravityStrength((EGravityStrength)((int)gravSlider.value)); });

        var speedslider = SpeedSlider.GetComponentInChildren<Slider>();
        speedslider.onValueChanged.AddListener(delegate { SetGameSpeed((EGameSpeed)((int)speedslider.value)); });

        var shiftSlider = ShiftSlider.GetComponentInChildren<Slider>();
        shiftSlider.onValueChanged.AddListener(delegate { SetShiftPipesMode((EShiftPipesMode)((int)shiftSlider.value)); });

        var mirrorslider = MirrorSlider.GetComponentInChildren<Slider>();
        mirrorslider.onValueChanged.AddListener(delegate { SetMirrorMode((EMirrorMode)((int)mirrorslider.value)); });

        //Adjust sliders to correct positions
        VolumeSlider.GetComponentInChildren<Slider>().value = GameSettings.GetMasterVolume();
        gravSlider.value = (int)GameSettings.GetGravityStrength();
        speedslider.value = (int)GameSettings.GetGameSpeed();
        shiftSlider.value = (int)GameSettings.GetShiftPipesMode();
        mirrorslider.value = (int)GameSettings.GetMirrorMode();
    }

    public void SetMasterVolume(float volume)
    {
        GameSettings.SetMasterVolume(volume);
    }

    public void SetGravityStrength(EGravityStrength gravity)
    {
        GameSettings.SetGravityStrength(gravity);

        //Update Text
        UpdateGravityText(gravity);
    }

    public void SetGameSpeed(EGameSpeed speed)
    {
        GameSettings.SetGameSpeed(speed);

        //Update Text
        UpdateGameSpeedText(speed);
    }

    public void SetShiftPipesMode(EShiftPipesMode mode)
    {
        GameSettings.SetShiftPipesMode(mode);

        //Update Text
        UpdateShiftPipesModeText(mode);
    }

    public void SetMirrorMode(EMirrorMode mode)
    {
        GameSettings.SetMirrorMode(mode);

        //Update Text
        UpdateMirrorModeText(mode);
    }


    private void UpdateGravityText(EGravityStrength gravity)
    {
        GravitySlider.GetComponentInChildren<Text>().text = "Gravity: " + gravity.ToString();
    }

    private void UpdateGameSpeedText(EGameSpeed speed)
    {
        SpeedSlider.GetComponentInChildren<Text>().text = "Game Speed: " + (speed.ToString().Replace("_", " "));
    }

    private void UpdateShiftPipesModeText(EShiftPipesMode mode)
    {
        ShiftSlider.GetComponentInChildren<Text>().text = "Shift Pipes: " + (mode.ToString().Replace("_", " "));
    }

    private void UpdateMirrorModeText(EMirrorMode mode)
    {
        MirrorSlider.GetComponentInChildren<Text>().text = "Mirror Mode: " + (mode.ToString().Replace("_", " "));
    }
}

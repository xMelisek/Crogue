using UnityEngine;
using UnityEngine.Rendering;

public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject options;
    private float savedTimeScale;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void DashClick()
    {
        var player = FindObjectOfType<PlayerBehaviour>();
        if ((bool)player) player.Dash();
    }

    public void PauseClick(bool trigger)
    {
        pause.SetActive(trigger);
        savedTimeScale = trigger ? Time.timeScale : savedTimeScale;
        Time.timeScale = trigger ? 0 : savedTimeScale;
    }

    public void OptionsClick(bool trigger)
    {
        options.SetActive(trigger);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ChangePost(bool trigger)
    {
        Volume vol = Camera.main.GetComponent<Volume>();
        vol.enabled = trigger;
    }
}

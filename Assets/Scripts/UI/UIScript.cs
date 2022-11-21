using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public bool HUD 
    {
        get => hud.activeSelf;
        set => hud.SetActive(value);
    }

    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private Slider healthBar;

    private float savedTimeScale;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        FindObjectOfType<PlayerBehaviour>().OnHealthUpdate += UpdateHealthBar;
    }

    #region Buttons
    public void DashClick()
    {
        var player = FindObjectOfType<PlayerBehaviour>();
        if ((bool)player) player.Dash();
    }

    //Toggle pause menu, HUD and time scale
    public void PauseClick(bool trigger)
    {
        HUD = !trigger;
        pause.SetActive(trigger);
        savedTimeScale = trigger ? Time.timeScale : savedTimeScale;
        Time.timeScale = trigger ? 0 : savedTimeScale;
    }

    //Toggle options menu
    public void OptionsClick(bool trigger)
    {
        options.SetActive(trigger);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }

    //Change Post processing of the camera
    public void ChangePost(bool trigger)
    {
        Volume vol = Camera.main.GetComponent<Volume>();
        vol.enabled = trigger;
    }
    #endregion

    public IEnumerator ToggleDeathUI()
    {
        yield return new WaitForSeconds(5f);
        deathUI.SetActive(true);
        yield break;
    }

    private void UpdateHealthBar(float[] health)
    {
        healthBar.maxValue = health[1];
        healthBar.value = health[0];
    }
}

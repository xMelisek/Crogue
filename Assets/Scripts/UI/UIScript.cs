using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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

    private float savedTimeScale;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    #region Buttons
    public void DashClick()
    {
        var player = FindObjectOfType<PlayerBehaviour>();
        if ((bool)player) player.Dash();
    }

    public void PauseClick(bool trigger)
    {
        HUD = !trigger;
        pause.SetActive(trigger);
        savedTimeScale = trigger ? Time.timeScale : savedTimeScale;
        Time.timeScale = trigger ? 0 : savedTimeScale;
    }

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
}

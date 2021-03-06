using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtility : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HideBannerAds()
    {
        AdManager.Instance.HideBanner();
    }

    public void MuteToggleBackgroundMusic()
    {
        AudioManager.Instance.ToggleBackgroundMusic();
    }

    public void MuteToggleSoundFx()
    {
        AudioManager.Instance.ToggleSoundFx();
    }



}

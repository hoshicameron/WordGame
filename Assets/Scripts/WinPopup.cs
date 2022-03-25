using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopup : MonoBehaviour
{
    [SerializeField] private GameObject winPopup=null;

    private void Start()
    {
        winPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.BoardCompletedEvent+=ShowWinPopup;
        AdManager.OnInterstitialAdsClosed+=InterstitialAdsCompleted;
    }
    private void OnDisable()
    {
        GameEvents.BoardCompletedEvent-=ShowWinPopup;
        AdManager.OnInterstitialAdsClosed-=InterstitialAdsCompleted;
    }

    private void InterstitialAdsCompleted()
    {
        // Todo stop all sounds
    }
    private void ShowWinPopup()
    {
        AdManager.Instance.HideBanner();
        winPopup.SetActive(true);
    }

    public void LoadNextLevel()
    {
        AdManager.Instance.ShowInterstitialAd();
        GameEvents.CallLoadNextLevel();
    }




}

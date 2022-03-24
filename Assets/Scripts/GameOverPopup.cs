using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private GameObject continueGameAfterAdsButton;

    private void Start()
    {
        gameOverPopup.SetActive(false);
        continueGameAfterAdsButton.GetComponent<Button>().interactable = false;
    }

    private void OnEnable()
    {
        GameEvents.GameOverEvent+=OnGameOverEvent;
    }

    private void OnDisable()
    {
        GameEvents.GameOverEvent-=OnGameOverEvent;

    }
    private void OnGameOverEvent()
    {
        gameOverPopup.SetActive(true);
        continueGameAfterAdsButton.GetComponent<Button>().interactable = false;
    }

 }

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
        GameEvents.BoardCompletedEvent+=OnBoardCompletedEvent;
    }

    private void OnDisable()
    {
        GameEvents.BoardCompletedEvent-=OnBoardCompletedEvent;
    }
    private void OnBoardCompletedEvent()
    {
        winPopup.SetActive(true);
    }

    public void LoadNextLevel()
    {
        GameEvents.CallLoadNextLevel();
    }




}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] private GameData currentGameData = null;
    [SerializeField] private TextMeshProUGUI timerText = null;

    private float timeleft;
    private float minutes;
    private float seconds;
    private float oneSecondDown;
    private bool timeOut;
    private bool timerStop;

    private void Start()
    {
        timerStop = false;
        timeOut = false;
        timeleft = currentGameData.selectedBoardData.timeInSeconds;
        oneSecondDown = timeleft - 1f;
    }

    private void OnEnable()
    {
        GameEvents.BoardCompletedEvent += StopTimer;
    }

    private void OnDisable()
    {
        GameEvents.BoardCompletedEvent -= StopTimer;
    }

    private void Update()
    {
        if (timerStop == false)
            timeleft -= Time.deltaTime;

        if (timeleft <= oneSecondDown)
            oneSecondDown = timeleft - 1f;
    }

    public void StopTimer()
    {
        timerStop = true;
    }

    private void OnGUI()
    {
        if (timeOut == false)
        {
            if (timeleft > 0)
            {
                minutes = Mathf.Floor(timeleft / 60);
                seconds = Mathf.RoundToInt(timeleft%60);
                timerText.SetText($"{minutes:00}:{seconds:00}");
            } else
            {
                timerStop = true;
                ActivateGameOverGUI();
            }
        }
    }

    private void ActivateGameOverGUI()
    {
        GameEvents.CallGameOverEvent();
        timeOut = true;
    }
}

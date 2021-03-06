using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static event Action EnableSquareSelectionEvent;

    public static void CallEnableSquareSelectionEvent()
    {
        EnableSquareSelectionEvent?.Invoke();
    }

    //************************************************************

    public static event Action DisableSquareSelectionEvent;

    public static void CallDisableSquareSelectionEvent()
    {
        DisableSquareSelectionEvent?.Invoke();
    }

    //************************************************************

    public static event Action<Vector3> SelectSquareEvent;

    public static void CallSelectSquareEvent(Vector3 position)
    {
        SelectSquareEvent?.Invoke(position);
    }

    //************************************************************

    public static event Action<string,Vector3,int> CheckSquareEvent;

    public static void CallCheckSquareEvent(string letter,Vector3 squarePosition,int squareIndex)
    {
        CheckSquareEvent?.Invoke(letter,squarePosition,squareIndex);
    }

    //************************************************************

    public static event Action ClearSelectionEvent;

    public static void CallClearSelectionEvent()
    {
        ClearSelectionEvent?.Invoke();
    }

    //************************************************************

    public static event Action<string, List<int>> CorrectWordEvent;

    public static void CallCorrectWordEvent(string word, List<int> squareIndexes)
    {
        CorrectWordEvent?.Invoke(word,squareIndexes);
    }

    public static event Action BoardCompletedEvent;

    public static void CallBoardCompletedEvent()
    {
        BoardCompletedEvent?.Invoke();
    }

    //************************************************************
    public static event Action UnlockNextCategoryEvent;

    public static void CallUnlockNextCategoryEvent()
    {
        UnlockNextCategoryEvent?.Invoke();
    }

    //************************************************************
    public static event Action LoadNextLevel;

    public static void CallLoadNextLevel()
    {
        LoadNextLevel?.Invoke();
    }

    //************************************************************

    public static event Action GameOverEvent;

    public static void CallGameOverEvent()
    {
        GameOverEvent?.Invoke();
    }

    //************************************************************

    public static event Action SoundFxEvent;

    public static void CallSoundFxEvent()
    {
        SoundFxEvent?.Invoke();
    }


}

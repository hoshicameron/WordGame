using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    public int SquareIndex { get; set; }

    private AlphabetData.letterData normalLetterData;        // Normal square sprite
    private AlphabetData.letterData selectedLetterData;      // Selected square sprite
    private AlphabetData.letterData correctLetterData;       // Correct square sprite

    private SpriteRenderer displayedImage;

    private bool selected = false;
    private bool clicked = false;
    private bool correct = false;

    private int index = -1;


    public void SetIndex(int index)
    {
        this.index = index;
    }

    public int GetIndex()
    {
        return index;
    }

    private AudioSource audioSource;

    private void OnEnable()
    {
        GameEvents.EnableSquareSelectionEvent+=OnEnableSquareSelectionEvent;
        GameEvents.DisableSquareSelectionEvent+=OnDisableSquareSelectionEvent;
        GameEvents.SelectSquareEvent+=OnSelectSquareEvent;
        GameEvents.CorrectWordEvent += CorrectWord;

    }

    private void OnDisable()
    {
        GameEvents.EnableSquareSelectionEvent-=OnEnableSquareSelectionEvent;
        GameEvents.DisableSquareSelectionEvent-=OnDisableSquareSelectionEvent;
        GameEvents.SelectSquareEvent-=OnSelectSquareEvent;
        GameEvents.CorrectWordEvent -= CorrectWord;

    }

    private void OnSelectSquareEvent(Vector3 position)
    {
        if (transform.position == position)
            displayedImage.sprite = selectedLetterData.image;
    }

    private void OnDisableSquareSelectionEvent()
    {
        // deselect square
        selected = false;
        clicked = false;

        // if player found correct word then change the  square sprite to correct image
        // otherwise make it normal
        displayedImage.sprite = correct ? correctLetterData.image : normalLetterData.image;
    }

    private void OnEnableSquareSelectionEvent()
    {
        clicked = true;
        selected = false;
    }

    private void CorrectWord(string word, List<int> squareIndexes)
    {
        // Square is selected and it's in correct word
        if (selected && squareIndexes.Contains(index))
        {
            correct = true;
            displayedImage.sprite = correctLetterData.image;
        }

        selected = false;
        clicked = false;
    }
    private void OnMouseDown()
    {
        OnEnableSquareSelectionEvent();
        GameEvents.CallEnableSquareSelectionEvent();
        ChecksSquare();
        displayedImage.sprite = selectedLetterData.image;
    }

    private void OnMouseEnter()
    {
        ChecksSquare();
    }

    private void OnMouseUp()
    {
        // Clear all selected squares
        GameEvents.CallClearSelectionEvent();

        // Trigger DisableSquareSelectionEvent on all selected squares
        GameEvents.CallDisableSquareSelectionEvent();


    }

    public void ChecksSquare()
    {
        // If mouse over the current square and clicked then add
        // letter to word string in WordChecker class

        if (!selected && clicked)
        {
            if(!AudioManager.Instance.IsSoundFxMuted())
                audioSource.Play();

            selected = true;
            GameEvents.CallCheckSquareEvent(normalLetterData.letter,transform.position,index);
        }
    }

    private void Start()
    {
        displayedImage = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Set square sprite image
    /// </summary>
    /// <param name="normalLetterData"></param>
    /// <param name="selectedLetterData"></param>
    /// <param name="correctLetterData"></param>
    public void SetSprite(AlphabetData.letterData normalLetterData,
                          AlphabetData.letterData selectedLetterData,
                          AlphabetData.letterData correctLetterData)
    {
        this.normalLetterData = normalLetterData;
        this.selectedLetterData = selectedLetterData;
        this.correctLetterData = correctLetterData;

        GetComponent<SpriteRenderer>().sprite = this.normalLetterData.image;
    }
}

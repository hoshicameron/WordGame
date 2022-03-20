using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    public int SquareIndex { get; set; }

    private AlphabetData.letterData normalLetterData;
    private AlphabetData.letterData selectedLetterData;
    private AlphabetData.letterData correctLetterData;

    private SpriteRenderer displayedImage;

    private bool selected = false;
    private bool clicked = false;

    private int index = -1;

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public int GetIndex()
    {
        return index;
    }


    private void OnEnable()
    {
        GameEvents.EnableSquareSelectionEvent+=OnEnableSquareSelectionEvent;
        GameEvents.DisableSquareSelectionEvent+=OnDisableSquareSelectionEvent;
        GameEvents.SelectSquareEvent+=OnSelectSquareEvent;

    }

    private void OnDisable()
    {
        GameEvents.EnableSquareSelectionEvent-=OnEnableSquareSelectionEvent;
        GameEvents.DisableSquareSelectionEvent-=OnDisableSquareSelectionEvent;
        GameEvents.SelectSquareEvent-=OnSelectSquareEvent;

    }

    private void OnSelectSquareEvent(Vector3 position)
    {
        if (transform.position == position)
            displayedImage.sprite = selectedLetterData.image;
    }

    private void OnDisableSquareSelectionEvent()
    {
        selected = false;
        clicked = false;
    }

    private void OnEnableSquareSelectionEvent()
    {
        clicked = true;
        selected = false;
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
        GameEvents.CallClearSelectionEvent();
        GameEvents.CallDisableSquareSelectionEvent();
    }

    public void ChecksSquare()
    {
        if (!selected && clicked)
        {
            selected = true;
            GameEvents.CallCheckSquareEvent(normalLetterData.letter,transform.position,index);
        }
    }

    private void Start()
    {
        displayedImage = GetComponent<SpriteRenderer>();
    }

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

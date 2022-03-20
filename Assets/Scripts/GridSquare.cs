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

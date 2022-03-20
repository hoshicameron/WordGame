using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingWordsList : MonoBehaviour
{
    [SerializeField] private GameData currentGameData=null;
    [SerializeField] private GameObject searchingWordPrefab=null;
    [SerializeField] private float offset = 0;
    [SerializeField] private int maxColumns = 5;
    [SerializeField] private int maxRows = 4;

    private int columns = 2;
    private int rows = 0;
    private int wordsNumber;

    private List<GameObject> words=new List<GameObject>();

    private void Start()
    {
        wordsNumber = currentGameData.selectedBoardData.searchWords.Count;

        if (wordsNumber < columns)     rows = 1;
        else                           CalculateColumnsAndRowsNumber();

        CreateWordsObjects();
        SetWordsPosition();
    }

    private void SetWordsPosition()
    {
        var squareRect = words[0].GetComponent<RectTransform>();
        var wordOffset=new Vector2
        {
            x = squareRect.rect.width * squareRect.transform.localScale.x + offset,
            y = squareRect.rect.height * squareRect.transform.localScale.y + offset
        };

        int columnNumber = 0;
        int rowNumber = 0;
        var startPosition = GetFirstSquarePosition();


        foreach (var word in words)
        {
            if (columnNumber+1 > columns)
            {
                columnNumber = 0;
                rowNumber++;
            }

            var positionX = startPosition.x + wordOffset.x * columnNumber;
            var positionY = startPosition.y - wordOffset.y * rowNumber;

            word.GetComponent<RectTransform>().localPosition=new Vector2(positionX,positionY);
            columnNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition=new Vector2(0f,transform.position.y);
        var squareRect = words[0].GetComponent<RectTransform>();
        var parentRect = GetComponent<RectTransform>();
        var squareSize = new Vector2(0f,0f);

        squareSize.x = squareRect.rect.width * squareRect.transform.localScale.x + offset;
        squareSize.y = squareRect.rect.height * squareRect.transform.localScale.y + offset;

        // make sure they are in the center
        var shiftBy = (parentRect.rect.width - (squareSize.x * columns)) * 0.5f;

        startPosition.x = ((parentRect.rect.width - squareSize.x) * 0.5f) * (-1);
        startPosition.x += shiftBy;

        startPosition.y = (parentRect.rect.height - squareSize.y) * 0.5f;

        return startPosition;
    }

    private void CalculateColumnsAndRowsNumber()
    {
        do
        {
            columns++;
            rows = wordsNumber / columns;
        } while (rows>=maxRows);

        if (columns > maxColumns)
        {
            columns = maxColumns;
            rows = wordsNumber / columns;
        }
    }

    private bool TryIncreaseColumnNumber()
    {
        columns++;
        rows = wordsNumber / columns;

        if (columns > maxColumns)
        {
            columns = maxColumns;
            rows = wordsNumber / columns;

            return false;
        }

        if (wordsNumber % columns>0) rows++;

        return true;
    }

    private void CreateWordsObjects()
    {
        var squareScale = GetSquareScale(new Vector3(1f, 1f, 0.1f));

        for (int i = 0; i < wordsNumber; i++)
        {
            words.Add(Instantiate(searchingWordPrefab)as GameObject);
            words[i].transform.SetParent(transform);
            words[i].GetComponent<Transform>().localScale = squareScale;
            words[i].GetComponent<RectTransform>().localPosition=new Vector3(0f,0f,0f);
        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;        // How much we will scale up or down specific square

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;

                return finalScale;
            }

        }

        return finalScale;
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = searchingWordPrefab.GetComponent<RectTransform>();
        var parentRect = GetComponent<RectTransform>();

        var squareSize = new Vector2(0f, 0f);

        squareSize.x = (squareRect.rect.width * targetScale.x) + offset;
        squareSize.y = (squareRect.rect.height * targetScale.y) + offset;

        var totalSquareHeight = squareSize.y * rows;

        // make sure all the square fit in parent rectangle
        if (totalSquareHeight > parentRect.rect.height)
        {
            while (totalSquareHeight > parentRect.rect.height)
            {
                if (TryIncreaseColumnNumber())
                {
                    totalSquareHeight = squareSize.y * rows;
                } else
                {
                    return true; // if reached limit then scale down squares
                }
            }
        }

        var totalSquareWidth = squareSize.x * columns;

        if (totalSquareWidth > parentRect.rect.width)
            return true;

        return false;
    }
}


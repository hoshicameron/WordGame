using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{
    [SerializeField] private GameData currentGameData;
    [SerializeField] private GameObject gridSquarePrefab;
    [SerializeField] private AlphabetData alphabetData;

    [SerializeField] private float squareOffset = 0.0f;
    [SerializeField] private float topPosition = 0.0f;

    private List<GameObject> squareList=new List<GameObject>();

    private void Start()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        if (currentGameData != null)
        {
            var squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));
            foreach (var squares in currentGameData.selectedBoardData.board)
            {
                foreach (var squareLetter in squares.row)
                {
                    var normalLetterData = alphabetData.alphabetNormal.Find(data => data.letter == squareLetter);
                    var selectedLetterData = alphabetData.alphabetHighlighted.Find(data => data.letter == squareLetter);
                    var correctLetterData = alphabetData.alphabetWrong.Find(data => data.letter == squareLetter);

                    if (normalLetterData.image == null || selectedLetterData.image == null)
                    {
                        Debug.LogError("All fields in your array must have some letters. Please fill up with random letter button" +squareLetter);

                        // Stop Editor Immediately
                        #if(UNITY_EDITOR)

                            if (UnityEditor.EditorApplication.isPlaying)
                                UnityEditor.EditorApplication.isPlaying = false;
                        #endif

                    } else
                    {
                        squareList.Add(Instantiate(gridSquarePrefab));
                        squareList[squareList.Count - 1].GetComponent<GridSquare>()
                            .SetSprite(normalLetterData, selectedLetterData, correctLetterData);

                        squareList[squareList.Count - 1].transform.SetParent(this.transform);
                        squareList[squareList.Count - 1].GetComponent<Transform>().position=new Vector3(0f,0f,0f);
                        squareList[squareList.Count - 1].transform.localScale = squareScale;
                    }
                }
            }
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

    /// <summary>
    /// If our board not fit into the screen then scale down all the squares
    /// </summary>
    /// <param name="targetScale"></param>
    /// <returns></returns>
    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize=new Vector2(0f,0f);
        var startPosition=new Vector2(0f,0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.y) + squareOffset;

        var midWidthPosition = ((currentGameData.selectedBoardData.columns * squareSize.x) * 0.5f) * 0.01f;
        var midHeightPosition = ((currentGameData.selectedBoardData.rows * squareSize.y) * 0.5f) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y = midHeightPosition;

        return (startPosition.x < GetHalfScreenWidth() * -1) || startPosition.y > topPosition;
    }

    private float GetHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;

        return width / 2;
    }

    private void SetSquaresPosition()
    {
        var squareRect = squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squareList[0].GetComponent<Transform>();

        var offset = new Vector2
        {
            x = ((squareRect.width * squareTransform.localScale.x) + squareOffset) * 0.01f,
            y = ((squareRect.height * squareTransform.localScale.y) + squareOffset) * 0.01f
        };

        var startPosition = GetFirstSquarePosition();

        int columnNumber = 0;
        int rowNumber = 0;

        foreach (var square in squareList)
        {
            if (rowNumber + 1 > currentGameData.selectedBoardData.rows)
            {
                columnNumber++;
                rowNumber=0;
            }

            var positionX = startPosition.x + offset.x * columnNumber;
            var positionY = startPosition.y - offset.y * rowNumber;

            square.GetComponent<Transform>().position=new Vector2(positionX,positionY);
            rowNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition=new Vector2(0f,transform.position.y);
        var squareRect = squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squareList[0].GetComponent<Transform>();
        var squareSize=new Vector2(0f,0f);

        squareSize.x = squareRect.width * squareTransform.localScale.x;
        squareSize.y = squareRect.height * squareTransform.localScale.y;

        var midWidthPosition = (((currentGameData.selectedBoardData.columns - 1) * squareSize.x) * 0.5f) * 0.01f;
        var midHeightPosition=(((currentGameData.selectedBoardData.rows - 1) * squareSize.y) * 0.5f) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y += midHeightPosition;

        return startPosition;
    }
}

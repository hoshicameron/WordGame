using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordChecker : MonoBehaviour
{
    [SerializeField] private GameData currentGameData = null;
    [SerializeField] private GameLevelData levelData = null;

    private string word;

    private int assignedPoints=0;
    private int completedWords = 0;
    private Ray2D rayUp, rayRight, rayDown, rayLeft;
    private Ray2D rayDiagonalLeftUp, rayDiagonalLeftDown;
    private Ray2D rayDiagonalRightUp, rayDiagonalRightDown;
    private Ray2D currentRay=new Ray2D();
    private Vector3 rayStartPosition=Vector3.zero;
    private List<int> correctSquareList=new List<int>();

    private void OnEnable()
    {
        GameEvents.CheckSquareEvent+=SquareSelected;
        GameEvents.ClearSelectionEvent+=ClearSelection;
        GameEvents.LoadNextLevel+=OnLoadNextLevel;
    }

    private void OnDisable()
    {
        GameEvents.CheckSquareEvent-=SquareSelected;
        GameEvents.ClearSelectionEvent-=ClearSelection;
        GameEvents.LoadNextLevel-=OnLoadNextLevel;
    }

    private void OnLoadNextLevel()
    {
        SceneManager.LoadScene("Scene_Game");
    }


    private void Update()
    {
        if (assignedPoints > 0 && Application.isEditor)
        {
            Debug.DrawRay(rayUp.origin,rayUp.direction*4);
            Debug.DrawRay(rayDown.origin,rayDown.direction*4);
            Debug.DrawRay(rayLeft.origin,rayLeft.direction*4);
            Debug.DrawRay(rayRight.origin,rayRight.direction*4);
            Debug.DrawRay(rayDiagonalLeftUp.origin,rayDiagonalLeftUp.direction*4);
            Debug.DrawRay(rayDiagonalLeftDown.origin,rayDiagonalLeftDown.direction*4);
            Debug.DrawRay(rayDiagonalRightUp.origin,rayDiagonalRightUp.direction*4);
            Debug.DrawRay(rayDiagonalRightDown.origin,rayDiagonalRightDown.direction*4);
        }
    }

    private void ClearSelection()
    {
        assignedPoints = 0;
        correctSquareList.Clear();
        word = string.Empty;
    }

    private void SquareSelected(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (assignedPoints == 0)
        {
            rayStartPosition = squarePosition;
            correctSquareList.Add(squareIndex);
            word += letter;

            rayUp = new Ray2D(new Vector2(squarePosition.x, squarePosition.y), Vector2.up);
            rayDown = new Ray2D(new Vector2(squarePosition.x, squarePosition.y), Vector2.down);
            rayRight = new Ray2D(new Vector2(squarePosition.x, squarePosition.y), Vector2.right);
            rayLeft = new Ray2D(new Vector2(squarePosition.x, squarePosition.y), Vector2.left);
            rayDiagonalLeftUp=new Ray2D(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1,1));
            rayDiagonalLeftDown=new Ray2D(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1,-1));
            rayDiagonalRightUp=new Ray2D(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1,1));
            rayDiagonalRightDown=new Ray2D(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1,-1));
        }
        else if (assignedPoints == 1)
        {
            correctSquareList.Add(squareIndex);
            currentRay = selectRay(rayStartPosition, squarePosition);
            GameEvents.CallSelectSquareEvent(squarePosition);
            word += letter;
            CheckWord();
        } else
        {
            if (IsPointOnTheRay(currentRay, squarePosition))
            {
                correctSquareList.Add(squareIndex);
                GameEvents.CallSelectSquareEvent(squarePosition);
                word += letter;
                CheckWord();
            }
        }

        assignedPoints++;


    }

    private void CheckWord()
    {
        foreach (var searchWord in currentGameData.selectedBoardData.searchWords)
        {
            if (word==searchWord.word)
            {
                GameEvents.CallCorrectWordEvent(word,correctSquareList);
                completedWords++;
                word = string.Empty;
                correctSquareList.Clear();
                CheckBoardCompleted();
                return;
            }
        }
    }

    /// <summary>
    /// Check if the selected square on the ray
    /// </summary>
    /// <param name="currentRay"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool IsPointOnTheRay(Ray2D currentRay,Vector3 point)
    {
        var hits = Physics2D.RaycastAll(this.currentRay.origin, currentRay.direction,100.0f);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
                return true;
        }

        return false;
    }

    private Ray2D selectRay(Vector2 firstPosition, Vector2 secondPosition)
    {
        var direction = (secondPosition - firstPosition).normalized;
        float tolerance = 0.01f;

        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - 1f) < tolerance)
        {
            return rayUp;
        }

        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - (-1f)) < tolerance)
        {
            return rayDown;
        }

        if (Mathf.Abs(direction.x - 1f) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return rayRight;
        }

        if (Mathf.Abs(direction.x - (-1f)) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return rayLeft;
        }

        if (direction.x < 0f && direction.y > 0f) return rayDiagonalLeftUp;
        if (direction.x < 0f && direction.y < 0f) return rayDiagonalLeftDown;
        if (direction.x > 0f && direction.y > 0f) return rayDiagonalRightUp;
        if (direction.x > 0f && direction.y < 0f) return rayDiagonalRightDown;

        return rayDown;
    }


    private void CheckBoardCompleted()
    {
        bool loadNextCategory = false;

        if (currentGameData.selectedBoardData.searchWords.Count == completedWords)
        {
            // Save current level progress
            var categoryName = currentGameData.selectedCategoryName;
            var currentBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(categoryName);
            var nextBoardIndex = -1;
            var currentCategoryIndex = 0;
            bool readNextLevelName = false;

            for (int i = 0; i < levelData.data.Count; i++)
            {
                if (readNextLevelName)
                {
                    nextBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(levelData.data[i].categoryName);
                    readNextLevelName = false;
                }

                // if current index is the category index then next index is the category to load
                if (levelData.data[i].categoryName == categoryName)
                {
                    readNextLevelName = true;
                    currentCategoryIndex = i;
                }
            }

            // if not at the end of category boards go to next level in the current category
                var currentLevelSize = levelData.data[currentCategoryIndex].BoardDatas.Count;
                if(currentBoardIndex<currentLevelSize)
                    currentBoardIndex++;

                DataSaver.SaveCategoryData(categoryName,currentBoardIndex);

                // Unlock next category
                if (currentBoardIndex >= currentLevelSize)
                {
                    currentCategoryIndex++;
                    // if this is not the last category
                    if (currentCategoryIndex < levelData.data.Count)
                    {
                        categoryName = levelData.data[currentCategoryIndex].categoryName;
                        currentBoardIndex = 0;
                        loadNextCategory = true;

                        if(nextBoardIndex<0)
                            DataSaver.SaveCategoryData(categoryName,currentBoardIndex);

                    } else
                    {
                        SceneManager.LoadScene("Scene_SelectCategory");
                    }
                } else
                {
                    GameEvents.CallBoardCompletedEvent();
                }

                if (loadNextCategory)
                    GameEvents.CallUnlockNextCategoryEvent();

            }
        }
    }


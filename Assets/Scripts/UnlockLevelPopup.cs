using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelPopup : MonoBehaviour
{
    [System.Serializable]
    public struct CategoryName
    {
        public string name;
        public Sprite sprite;
    }

    [SerializeField] private GameData currentGameData;
    [SerializeField] private List<CategoryName> categoryNames=new List<CategoryName>();
    [SerializeField] private GameObject winPopup;
    [SerializeField] private Image categoryNameImage;

    private void Start()
    {
        winPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.UnlockNextCategoryEvent+=UnlockNextCategory;
    }

    private void OnDisable()
    {
        GameEvents.UnlockNextCategoryEvent-=UnlockNextCategory;
    }

    private void UnlockNextCategory()
    {
        bool captureNext = false;

        foreach (var category in categoryNames)
        {
            if (captureNext)
            {
                categoryNameImage.sprite = category.sprite;
                captureNext = false;
                break;
            }

            if (category.name == currentGameData.selectedCategoryName)
            {
                captureNext = true;
            }
        }

        winPopup.SetActive(true);
    }
}

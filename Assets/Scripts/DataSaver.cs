using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DataSaver : MonoBehaviour
{
    /// <summary>
    /// Read current category index from player prefs with category name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int ReadCategoryCurrentIndexValues(string name)
    {
        var value = -1;
        if (PlayerPrefs.HasKey(name))
            value = PlayerPrefs.GetInt(name);

        return value;
    }

    /// <summary>
    /// Save current category index to player prefs
    /// </summary>
    /// <param name="categoryName"></param>
    /// <param name="currentIndex"></param>
    public static void SaveCategoryData(string categoryName, int currentIndex)
    {
        PlayerPrefs.SetInt(categoryName,currentIndex);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Clear all saved category indexes from player prefs
    /// </summary>
    /// <param name="levelData"></param>
    public static void ClearGameData(GameLevelData levelData)
    {
        print("Clear");
        foreach (var record in levelData.data)
        {
            PlayerPrefs.SetInt(record.categoryName,-1);
        }

        // Unlock The first level
        PlayerPrefs.SetInt(levelData.data[0].categoryName,0);
        PlayerPrefs.Save();
    }
}

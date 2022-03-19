using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_AlphabetData",menuName ="ScriptableObjects/AlphabetData")]
public class AlphabetData : ScriptableObject
{
    [System.Serializable]
    public class letterData
    {
        public string letter;
        public Sprite image;
    }

    public List<letterData> alphabetPlain=new List<letterData>();
    public List<letterData> alphabetNormal=new List<letterData>();
    public List<letterData> alphabetHighlighted=new List<letterData>();
    public List<letterData> alphabetWrong=new List<letterData>();

}

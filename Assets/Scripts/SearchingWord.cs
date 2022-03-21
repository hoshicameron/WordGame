using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchingWord : MonoBehaviour
{
   [SerializeField] private GameObject crossLine=null;
   [SerializeField] private TextMeshProUGUI wordText = null;

   private string _word;

   private void OnEnable()
   {
      GameEvents.CorrectWordEvent += CorrectWord;
   }

   private void OnDisable()
   {
      GameEvents.CorrectWordEvent -= CorrectWord;
   }

   private void Start()
   {

   }

   public void SetWordText(string word)
   {
      _word = word;
      wordText.SetText(_word);
   }

   private void CorrectWord(string word, List<int> squareIndexes)
   {
      if (_word == word)
      {
         crossLine.SetActive(true);
      }
   }
}

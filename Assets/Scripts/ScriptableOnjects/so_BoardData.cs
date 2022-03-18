using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


[CreateAssetMenu(fileName = "so_BordData" , menuName ="ScriptableObjects/BoardData" )]
public class so_BoardData : ScriptableObject
{
    [System.Serializable]
    public class SearchingWord
    {
        public string word;
    }

    [System.Serializable]
    public class BoardRow
    {
        public int _size;
        public string[] row;

        public BoardRow(){}

        public BoardRow(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            _size = size;
            row=new string[_size];
            ClearRow();
        }

        public void ClearRow()
        {

            for (int i = 0; i < row.Length; i++)
            {
                row[i]=string.Empty;
            }
        }
    }

    public float timeInSeconds;        //Time that player has for solving puzzle
    public int columns = 0;
    public int rows = 0;

    public BoardRow[] board;

    public void ClearWithEmptyString()
    {
        for (int i = 0; i < columns; i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board=new BoardRow[columns];

        for (int i = 0; i < columns; i++)
        {
            // Memory Allocation
            board[i]= new BoardRow(rows);
        }
    }

}

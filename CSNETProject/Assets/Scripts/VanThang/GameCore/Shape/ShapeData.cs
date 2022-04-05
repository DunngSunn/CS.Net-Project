using System;
using UnityEngine;

namespace VanThang
{
    [CreateAssetMenu(fileName = "Shape ", menuName = "Data/Shape")]
    [Serializable]
    public class ShapeData : ScriptableObject
    {
        #region Fields

        public int columns;
        public int rows;
        public int shapeID;
        public Row[] board;

        #endregion
        
        // 
        public void ClearBoard()
        {
            for (var i = 0; i < rows; i++)
            {
                board[i].CLearRow();
            }
        }

        //
        public void CreateNewBoard()
        {
            board = new Row[rows];

            for (var i = 0; i < rows; i++)
            {
                board[i] = new Row(columns);
            }
        }
    
        // Class thông tin của 1 hàng trong board
        [Serializable]
        public class Row
        {
            public int size;
            public bool[] columns;

            public Row(int size)
            {
                this.size = size;
                columns = new bool[size];
                CLearRow();
            }

            public void CLearRow()
            {
                for (var i = 0; i < size; i++)
                {
                    columns[i] = false;
                }
            }
        }
    }
}
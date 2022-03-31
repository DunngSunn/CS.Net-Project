using System.Collections.Generic;
using DunnGSunn;
using UnityEngine;

namespace GameCore
{
    public class GameBoardGenerator : SunMonoSingleton<GameBoardGenerator>
    {
        #region Fields

        public int totalRow;
        public int totalColumn;
        public int blockSpace = 5;
        public GameObject boardContent;
        public GameObject emptyBlock;

        private int _startPosX;
        private int _startPosY;
        private int _blockWidth;
        private int _blockHeight;

        public List<BoardBlock> BlockGrid { get; private set; }

        #endregion

        private void Start()
        {
            GenerateBoard();
        }

        #region Functions

        public void GenerateBoard()
        {
            BlockGrid = new List<BoardBlock>();
            
            _blockHeight = (int) emptyBlock.GetComponent<RectTransform>().sizeDelta.x;
            _blockWidth = (int) emptyBlock.GetComponent<RectTransform>().sizeDelta.y;

            _startPosX = -((totalColumn - 1) * (_blockHeight + blockSpace) / 2);
            _startPosY = (totalRow - 1) * (_blockWidth + blockSpace) / 2;

            var newPosX = _startPosX;
            var newPosY = _startPosY;

            for (var row = 0; row < totalRow; row++)
            {
                var thisRowCells = new List<BoardBlock>();
                for (var column = 0; column < totalColumn; column++)
                {
                    var newCell = GenerateNewBlock(row, column);
                    newCell.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(newPosX, newPosY, 0);
                    newPosX += (_blockWidth + blockSpace);
                    var thisCellInfo = newCell.GetComponent<BoardBlock>();
                    thisCellInfo.InitializeBlock(row, column);
                    thisRowCells.Add (thisCellInfo);
                }

                BlockGrid.AddRange(thisRowCells);
                newPosX = _startPosX;
                newPosY -= (_blockHeight + blockSpace);
            }
        }

        private GameObject GenerateNewBlock(int rowIndex, int columnIndex)
        {
            var newBlock = Instantiate(emptyBlock, boardContent.transform);
            newBlock.GetComponent<RectTransform>().sizeDelta = new Vector2((_blockWidth), (_blockHeight));
            newBlock.transform.localScale = Vector3.one;
            newBlock.transform.SetAsLastSibling();
            newBlock.name = "Block-" + rowIndex + "-" + columnIndex;
            return newBlock;
        }

        #endregion
    }
}
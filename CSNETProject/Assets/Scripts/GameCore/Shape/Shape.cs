using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Shape : MonoBehaviour
    {
        #region Fields

        [Header("References")] 
        public GameObject emptyBlock;
        
        public int blockSpace = 5;

        public int StartOffsetX { get; private set; }
        public int StartOffsetY { get; private set; }
        public ShapeBlock FirstBlock { get; private set; }
        public List<ShapeBlock> ShapeBlocks { get; private set; }

        private int _startPosX = 0;
        private int _startPosY = 0;
        private int _blockWidth = 0;
        private int _blockHeight = 0;
        private float _newPosX;
        private float _newPosY;

        #endregion

        public void CreateBlockList(ShapeData data)
        {
            ShapeBlocks = new List<ShapeBlock>();
            
            _blockHeight = (int) emptyBlock.GetComponent<RectTransform>().sizeDelta.x;
            _blockWidth = (int) emptyBlock.GetComponent<RectTransform>().sizeDelta.y;
            
            _startPosX = -((data.columns - 1) * (_blockHeight + blockSpace) / 2);
            _startPosY = (data.rows - 1) * (_blockWidth + blockSpace) / 2;
            
            _newPosX = _startPosX;
            _newPosY = _startPosY;

            for (var row = 0; row < data.rows; row++)
            {
                for (var column = 0; column < data.columns; column++)
                {
                    if (data.board[row].columns[column])
                    {
                        var newBlock = CreateNewBlock(row, column, _newPosX, _newPosY);
                        var blockInfo = new ShapeBlock(newBlock.transform, row, column);
                        ShapeBlocks.Add(blockInfo);
                    }
                    _newPosX += (_blockWidth + blockSpace);
                }
                _newPosX = _startPosX;
                _newPosY -= (_blockHeight + blockSpace);
            }
        
            gameObject.name = data.name;
            FirstBlock = ShapeBlocks[0];
            StartOffsetX = FirstBlock.rowID;
            StartOffsetY = FirstBlock.columnID;
        }
        
        private GameObject CreateNewBlock(int row, int column, float posX, float posY)
        {
            var newBlock = Instantiate(emptyBlock, transform);
            newBlock.transform.SetAsLastSibling();
            newBlock.transform.localPosition = new Vector3(posX, posY, 0f);
            newBlock.transform.localScale = Vector3.one;
            newBlock.name = "Block-" + row + "-" + column;
            return newBlock;
        }
    }

    [Serializable]
    public class ShapeBlock
    {
        public Transform block;
        public int rowID;
        public int columnID;

        public ShapeBlock(Transform _block, int _rowID, int _columnID)
        {
            block = _block;
            rowID = _rowID;
            columnID = _columnID;
        }
    }
}
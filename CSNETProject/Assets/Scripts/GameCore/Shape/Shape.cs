using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameCore
{
    public class Shape : MonoBehaviour
    {
        #region Fields

        [Header("References")] 
        public GameObject emptyBlock;
        
        [Space]
        public int blockSpace = 5;

        public int StartOffsetX { get; private set; }
        public int StartOffsetY { get; private set; }
        public ShapeBlock FirstBlock { get; private set; }
        public List<ShapeBlock> ShapeBlocks { get; private set; }

        private int _startPosX;
        private int _startPosY;
        private int _blockWidth;
        private int _blockHeight;
        private float _newPosX;
        private float _newPosY;

        #endregion

        // Tạo hình theo data đưa vào
        public void CreateBlockList(ShapeData data)
        {
            // Tạo list chứa các ô
            ShapeBlocks = new List<ShapeBlock>();
            
            // Lấy height và width của mỗi ô
            _blockHeight = (int) emptyBlock.GetComponent<RectTransform>().sizeDelta.x;
            _blockWidth = (int) emptyBlock.GetComponent<RectTransform>().sizeDelta.y;
            
            // Tính vị trí của ô đầu tiên
            _startPosX = -((data.columns - 1) * (_blockHeight + blockSpace) / 2);
            _startPosY = (data.rows - 1) * (_blockWidth + blockSpace) / 2;
            
            // Lưu trữ giá trị đầu của ô để sử dụng
            _newPosX = _startPosX;
            _newPosY = _startPosY;

            // Duyệt theo từng hàng
            for (var row = 0; row < data.rows; row++)
            {
                // Duyệt theo cột
                for (var column = 0; column < data.columns; column++)
                {
                    // Nếu như cột này có thể hiển thị thì tạo ô mới và đặt ở đây
                    if (data.board[row].columns[column])
                    {
                        // Tạo ô mới
                        var newBlock = CreateNewBlock(row, column, _newPosX, _newPosY);
                        
                        // Tạo data của ô
                        var blockInfo = new ShapeBlock(newBlock.transform, row, column);
                        
                        // Thêm vào list
                        ShapeBlocks.Add(blockInfo);
                    }
                    
                    // Tính vị trí tiếp theo của cột
                    _newPosX += (_blockWidth + blockSpace);
                }
                
                // Tính vị trí tiếp theo của hàng
                _newPosX = _startPosX;
                _newPosY -= (_blockHeight + blockSpace);
            }
        
            // Đổi tên và set giá trị cho hình
            gameObject.name = data.name;
            FirstBlock = ShapeBlocks[0];
            StartOffsetX = FirstBlock.rowID;
            StartOffsetY = FirstBlock.columnID;
        }
        
        // Tạo ô mới
        private GameObject CreateNewBlock(int row, int column, float posX, float posY)
        {
            // Khởi tạo ô mới từ object gốc
            var newBlock = Instantiate(emptyBlock, transform);
            
            // Đưa ô về vị trí con của khung chứa
            newBlock.transform.SetAsLastSibling();
            
            // Set vị trí
            newBlock.transform.localPosition = new Vector3(posX, posY, 0f);
            
            // Set scale
            newBlock.transform.localScale = Vector3.one;
            
            // Đổi tên
            newBlock.name = "Block-" + row + "-" + column;
            
            return newBlock;
        }
    }

    // Class thông tin của từng ô trong hình đặt
    [Serializable]
    public class ShapeBlock
    {
        public Transform block;
        public int rowID;
        public int columnID;

        public ShapeBlock(Transform block, int rowID, int columnID)
        {
            this.block = block;
            this.rowID = rowID;
            this.columnID = columnID;
        }
    }
}
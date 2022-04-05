using System.Collections.Generic;
using DunnGSunn;
using UnityEngine;

namespace NgocAnh
{
    public class GameBoardGenerator : SunMonoSingleton<GameBoardGenerator>
    {
        #region Fields

        // Thông tin của board
        public int totalRow;
        public int totalColumn;
        
        // Khoảng cách giữa các ô
        public int blockSpace = 5;
        
        // Object chứa tất cả các ô
        public GameObject boardContent;
        
        // Object ô trống
        public GameObject emptyBlock;

        // 
        private int _startPosX;
        private int _startPosY;
        private int _blockWidth;
        private int _blockHeight;

        // List các ô sau khi khởi tạo
        public List<BoardBlock> BlockGrid { get; private set; }

        #endregion

        #region Functions

        // Khởi tạo game board
        public void GenerateBoard()
        {
            // Tạo mới list các ô
            BlockGrid = new List<BoardBlock>();
            
            // Lấy height và width của mỗi ô
            _blockHeight = (int) emptyBlock.GetComponent<RectTransform>().sizeDelta.x;
            _blockWidth = (int) emptyBlock.GetComponent<RectTransform>().sizeDelta.y;

            // Tính vị trí của ô đầu tiên
            _startPosX = -((totalColumn - 1) * (_blockHeight + blockSpace) / 2);
            _startPosY = (totalRow - 1) * (_blockWidth + blockSpace) / 2;

            // Lưu trữ giá trị đầu của ô để sử dụng
            var newPosX = _startPosX;
            var newPosY = _startPosY;

            // Duyệt theo từng hàng
            for (var row = 0; row < totalRow; row++)
            {
                // Tạo mới list chứa các ô của hàng này
                var thisRowCells = new List<BoardBlock>();
                
                // Duyệt theo cột
                for (var column = 0; column < totalColumn; column++)
                {
                    // Tạo 1 ô mới
                    var newCell = GenerateNewBlock(row, column);
                    
                    // Set vị trí cho ô
                    newCell.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(newPosX, newPosY, 0);
                    
                    // Tính vị trí ô tiếp theo trong hàng này
                    newPosX += (_blockWidth + blockSpace);
                    
                    // Khởi tạo các hình ảnh hiển thị của ô
                    var thisCellInfo = newCell.GetComponent<BoardBlock>();
                    thisCellInfo.InitializeBlock(row, column);
                    
                    // Thêm ô này vào list
                    thisRowCells.Add (thisCellInfo);
                }

                // Thêm hàng này vào list to
                BlockGrid.AddRange(thisRowCells);
                
                // Tính vị trí của hàng tiếp theo
                newPosX = _startPosX;
                newPosY -= (_blockHeight + blockSpace);
            }
        }

        // Hàm khởi tạo ô mới
        private GameObject GenerateNewBlock(int rowIndex, int columnIndex)
        {
            // Tạo ô mới từ 1 object chung
            var newBlock = Instantiate(emptyBlock, boardContent.transform);
            
            // Set chiều dài và chiều rộng cho ô
            newBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(_blockWidth, _blockHeight);
            
            // Set scale của ô
            newBlock.transform.localScale = Vector3.one;
            
            // Đưa ô về vị trí con của khung chứa
            newBlock.transform.SetAsLastSibling();
            
            // Đổi tên cho ô
            newBlock.name = "Block-" + rowIndex + "-" + columnIndex;
            
            return newBlock;
        }
        
        // Clear board
        public void ClearBoard()
        {
            if (BlockGrid == null) return;
            
            foreach (var boardBlock in BlockGrid)
            {
                boardBlock.ClearBlock();
            }
        }

        #endregion
    }
}
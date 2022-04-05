using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DunnGSunn;
using NgocAnh;
using UnityEngine;
using UnityEngine.EventSystems;
using VanThang;

namespace GameCore
{
    public class Gameplay : SunMonoSingleton<Gameplay>, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region Fields

        // Singleton 
        private static ShapeGenerator ShapeGenerator => ShapeGenerator.Instance;
        private static GameBoardGenerator GameBoardGenerator => GameBoardGenerator.Instance;

        // Camera main
        private static Camera MainCam => Camera.main;

        // Fields
        private Shape _currentShape;
        private Transform _hittingBlock;
        private bool _canPlace;

        // List block để check lúc di chuyển shape
        private List<BoardBlock> _blockPlaces;

        #endregion

        #region Handler event

        // Sự kiện bấm xuống
        public void OnPointerDown(PointerEventData eventData)
        {
            // Kiểm tra có bấm vào 1 object nào không
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                // Lấy script shape trong object vừa bấm
                _currentShape = eventData.pointerCurrentRaycast.gameObject.GetComponent<Shape>();

                if (_currentShape != null)
                {
                    // Lấy vị trí và scale lại tỉ lệ
                    var pos = MainCam.ScreenToWorldPoint(eventData.position);
                    _currentShape.transform.DOScale(Vector3.one * .9f, .15f);
                    _currentShape.transform.DOMove(new Vector3(pos.x, pos.y + .5f, 0f), .15f);
                }
            }
        }

        // Sự kiện drag
        public void OnDrag(PointerEventData eventData)
        {
            if (_currentShape != null)
            {
                // Set vị trí theo điểm di chuyển
                var pos = MainCam.ScreenToWorldPoint(eventData.position);
                var newPos = new Vector3(pos.x, pos.y + .5f, 0f);
                _currentShape.transform.position = newPos;

                // Check có chạm vào ô nào không?
                var hit = Physics2D.Raycast(_currentShape.FirstBlock.block.position, Vector2.zero, 1f);
                if (hit.collider != null)
                {
                    if (_hittingBlock == null || hit.collider.transform != _hittingBlock)
                    {
                        _hittingBlock = hit.collider.transform;
                        _canPlace = CanPlaceShape(_hittingBlock);
                    }
                }
                else
                {
                    StopHighlighting();
                }
            }
        }

        // Sự kiện thả lên
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_currentShape != null)
            {
                // Check có thể đặt không
                if (_blockPlaces.Count > 0 && _canPlace)
                {
                    // Set shape vào trong khung
                    SetPlacedShape();

                    // Check game board
                    Invoke(nameof(CheckBoardStatus), .1f);
                }
                else
                {
                    // Dừng hiển thị highlighting
                    StopHighlighting();

                    // Đưa shape về vị trí cũ
                    _currentShape.transform.DOLocalMove(Vector3.zero, .15f);
                    _currentShape.transform.DOScale(Vector3.one * .7f, .15f).OnComplete(() =>
                    {
                        _currentShape = null;
                    });
                }
            }
        }

        #endregion

        private void Start()
        {
            _blockPlaces = new List<BoardBlock>();
        }

        #region Init gameplay

        // Chuẩn bị cho gameplay
        public void InitializeGameplay()
        {
            if (GameBoardGenerator.BlockGrid == null || GameBoardGenerator.BlockGrid.Count < 0)
            {
                GameBoardGenerator.GenerateBoard();
            }
            else
            {
                GameBoardGenerator.ClearBoard();
            }

            if (ShapeGenerator.ShapeInGame != null && ShapeGenerator.ShapeInGame.Count > 0)
            {
                foreach (var shape in ShapeGenerator.ShapeInGame)
                {
                    Destroy(shape.gameObject);
                }
            }

            ShapeGenerator.GenerateShape();
        }

        #endregion


        #region Control

        // Check shape có thể đặt được không.
        private bool CanPlaceShape(Component currentHittingBlock)
        {
            // Lấy script board block trong ô vừa chạm vào
            var currentBlock = currentHittingBlock.GetComponent<BoardBlock>();

            // Lấy row id và column id của ô
            var currentRowID = currentBlock.RowID;
            var currentColumnID = currentBlock.ColumnID;

            // Dừng hiển thị highlight trong ô
            StopHighlighting();

            // Kiểm tra shape có thể đặt được không
            var canPlaceShape = true;
            // Check từng ô theo hình dạng của shape
            foreach (var block in _currentShape.ShapeBlocks)
            {
                // Kiểm tra ô tương ứng với ô của shape
                var checkingBlock = GameBoardGenerator.BlockGrid.Find(b =>
                    b.RowID == currentRowID + block.rowID + _currentShape.StartOffsetX
                    && b.ColumnID == currentColumnID + block.columnID - _currentShape.StartOffsetY);

                // Check có ô tương ứng không
                if (checkingBlock == null || checkingBlock != null && checkingBlock.IsFilled)
                {
                    canPlaceShape = false;
                    _blockPlaces.Clear();
                    break;
                }
                else
                {
                    if (!_blockPlaces.Contains(checkingBlock))
                    {
                        _blockPlaces.Add(checkingBlock);
                    }
                }
            }

            // Nếu đặt được shape thì cho hiển thị highlight lại
            if (canPlaceShape)
            {
                SetHighLightImage();
            }

            return canPlaceShape;
        }

        // Hiển thị highlight theo các ô đã được check
        private void SetHighLightImage()
        {
            foreach (var block in _blockPlaces)
            {
                block.SetHighlightImage();
            }
        }

        // Dừng hiển thị highlight theo các ô đã được check
        private void StopHighlighting()
        {
            if (_blockPlaces != null && _blockPlaces.Count > 0)
            {
                foreach (var block in _blockPlaces)
                {
                    block.StopHighlighting();
                }
            }

            _hittingBlock = null;
            _blockPlaces?.Clear();
        }

        // Set shape vào trong khung
        private void SetPlacedShape()
        {
            if (_blockPlaces != null && _blockPlaces.Count > 0)
            {
                foreach (var block in _blockPlaces)
                {
                    block.StopHighlighting();
                    block.SetFilledBlock();
                }
            }

            // Destroy shape hiện tại
            ShapeGenerator.ShapeInGame.Remove(_currentShape);
            Destroy(_currentShape.gameObject);
            _currentShape = null;
        }

        // Lấy hàng đã full
        private List<BoardBlock> GetEntireRow(int rowID)
        {
            var thisRow = GameBoardGenerator.BlockGrid.FindAll(o => o.RowID == rowID);
            return thisRow.Any(block => !block.IsFilled) ? null : thisRow;
        }

        // Lấy cột đã full
        private List<BoardBlock> GetEntireColumn(int columnID)
        {
            var thisColumn = GameBoardGenerator.BlockGrid.FindAll(o => o.ColumnID == columnID);
            return thisColumn.Any(block => !block.IsFilled) ? null : thisColumn;
        }

        // Phá dòng full 
        private IEnumerator BreakThisLine(List<BoardBlock> breakingLine)
        {
            foreach (var block in breakingLine)
            {
                block.ClearBlockWithEffect();
                yield return new WaitForSeconds(0.02F);
            }

            yield return null;
        }

        // Phá tất cả dòng full và tính điểm
        private IEnumerator BreakAllCompletedLines(List<List<BoardBlock>> breakingRows, List<List<BoardBlock>> breakingColumns, int placingShapeBlockCount)
        {
            // Tính tổng hàng và cột có thể phá
            var totalBreakingLines = breakingRows.Count + breakingColumns.Count;

            if (totalBreakingLines == 1) {
                // AudioManager.Instance.PlaySound (lineClear1);
                // ScoreManager.Instance.AddScore (100 + (placingShapeBlockCount * 10));
            } else if (totalBreakingLines == 2) {
                // AudioManager.Instance.PlaySound (lineClear2);
                // ScoreManager.Instance.AddScore (300+ (placingShapeBlockCount * 10));
            } else if (totalBreakingLines == 3) {
                // AudioManager.Instance.PlaySound (lineClear3);
                // ScoreManager.Instance.AddScore (600+ (placingShapeBlockCount * 10));
            } else if (totalBreakingLines >= 4) {
                // AudioManager.Instance.PlaySound (lineClear4);
                if (totalBreakingLines == 4) {
                    // ScoreManager.Instance.AddScore (1000+ (placingShapeBlockCount * 10));
                } else {
                    // ScoreManager.Instance.AddScore ((300 * totalBreakingLines)+ (placingShapeBlockCount * 10));
                }
            }

            // Phá hàng
            if (breakingRows.Count > 0)
            {
                foreach (var thisLine in breakingRows)
                {
                    StartCoroutine(BreakThisLine(thisLine));
                }
            }

            if (breakingRows.Count > 0)
            {
                yield return new WaitForSeconds(0.1F);
            }

            // Phá cột
            if (breakingColumns.Count > 0)
            {
                foreach (var thisLine in breakingColumns)
                {
                    StartCoroutine(BreakThisLine(thisLine));
                }
            }

            // Fill shape vào các ô chứa
            ShapeGenerator.FillShapeContainer();
        }

        private void CheckBoardStatus()
        {
            // Lấy số lượng ô được đặt
            var placingShapeBlockCount = _blockPlaces.Count;
            
            // Các hàng, cột được đặt
            var updatedRows = new List<int>();
            var updatedColumns = new List<int>();

            // Các hàng, các cột đã full
            var breakingRows = new List<List<BoardBlock>>();
            var breakingColumns = new List<List<BoardBlock>>();

            // Update các hàng, các cột được đặt
            foreach (var b in _blockPlaces)
            {
                if (!updatedRows.Contains(b.RowID))
                {
                    updatedRows.Add(b.RowID);
                }

                if (!updatedColumns.Contains(b.ColumnID))
                {
                    updatedColumns.Add(b.ColumnID);
                }
            }

            // Clear list block được đặt
            _blockPlaces.Clear();

            // Check hàng được update có full không
            foreach (var rowID in updatedRows)
            {
                var currentRow = GetEntireRow(rowID);
                if (currentRow != null)
                {
                    breakingRows.Add(currentRow);
                }
            }

            // Check cột được update có full không
            foreach (var columnID in updatedColumns)
            {
                var currentColumn = GetEntireColumn(columnID);
                if (currentColumn != null)
                {
                    breakingColumns.Add(currentColumn);
                }
            }

            // Có hàng hoặc cột full thì phá tương ứng
            if (breakingRows.Count > 0 || breakingColumns.Count > 0)
            {
                StartCoroutine(BreakAllCompletedLines(breakingRows, breakingColumns, placingShapeBlockCount));
            }
            else
            {
                ShapeGenerator.FillShapeContainer();
                // ScoreManager.Instance.AddScore (10 * placingShapeBlockCount);
            }
            
            // Kiểm tra còn đặt được nữa không
            if (!CanExistingBlocksPlaced(ShapeGenerator.ShapeInGame))
            {
                Debug.Log("Game over");
            }
        }

        // Check shape còn lại còn đặt vào được không 
        private bool CanExistingBlocksPlaced(List<Shape> onBoardShapes)
        {
            foreach (var block in GameBoardGenerator.BlockGrid)
            {
                if (!block.IsFilled)
                {
                    foreach (var shape in onBoardShapes)
                    {
                        if (CheckShapeCanPlace(block, shape))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // Check còn đặt được nữa không với giá trị truyền vào là ô và shape
        private bool CheckShapeCanPlace(BoardBlock placingBlock, Shape placingShape)
        {
            var currentRowID = placingBlock.RowID;
            var currentColumnID = placingBlock.ColumnID;

            foreach (var shapeBlock in placingShape.ShapeBlocks)
            {
                var checkingBlock = GameBoardGenerator.BlockGrid.Find(b =>
                    b.RowID == currentRowID + shapeBlock.rowID + placingShape.StartOffsetX && b.ColumnID ==
                    currentColumnID + shapeBlock.columnID - placingShape.StartOffsetY);
                if (checkingBlock == null || (checkingBlock != null && checkingBlock.IsFilled))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
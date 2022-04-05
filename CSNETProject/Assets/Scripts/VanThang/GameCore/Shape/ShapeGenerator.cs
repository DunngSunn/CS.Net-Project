using System.Collections.Generic;
using DunnGSunn;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VanThang
{
    public class ShapeGenerator : SunMonoSingleton<ShapeGenerator>
    {
        #region Fields

        // Object gốc
        public GameObject emptyShape;

        // Các ô chứa shape
        public Transform[] shapeParents;

        // List data của shape
        public List<ShapeData> listShapeData;
        
        // Shape data pool
        private List<int> _shapeBlockPool;
        
        // Shape đang có trong gameplay
        public List<Shape> ShapeInGame { get; private set; }

        #endregion
        
        // Khởi tạo shape
        public void GenerateShape()
        {
            CreateShapeBlockPool();
            FillShapeContainer();
        }
        
        // Tạo bể chứa data của shape
        public void CreateShapeBlockPool()
        {
            // Tạo mới pool nếu đang null
            if (_shapeBlockPool == null)
            {
                _shapeBlockPool = new List<int>();
            }
            
            // Thêm data vào pool
            foreach (var shapeData in listShapeData)
            {
                var count = Random.Range(1, 4);
                for (var i = 0; i < count; i++)
                {
                    _shapeBlockPool.Add(shapeData.shapeID); 
                }
            }
        
            // Xáo trộn pool
            _shapeBlockPool.ShuffleList();
        }
        
        // Đặt shape vào ô chứa
        public void FillShapeContainer()
        {
            // Danh sách shape hiện có trong game null thì tạo mới
            if (ShapeInGame == null)
            {
                ShapeInGame = new List<Shape>();
            }
        
            // Check hiện đang có shape nào trong game không và thêm vào nó không có
            var isAllEmpty = ShapeInGame.Count <= 0;

            if (isAllEmpty)
            {
                foreach (var shapeContainer in shapeParents)
                {
                    AddShapeToContainer(shapeContainer);
                }
            }
        }
        
        // Thêm shape vào ô chứa
        private void AddShapeToContainer(Transform shapeParent)
        {
            // Check nếu pool chứa data shape null hoặc không có phần tử thì thêm mới
            if (_shapeBlockPool == null || _shapeBlockPool.Count <= 0)
            {
                CreateShapeBlockPool();
            }
            
            // Lấy data từ pool
            var shapeID = _shapeBlockPool[0];
            _shapeBlockPool.RemoveAt(0);
            
            // Tạo shape
            var shape = GenerateShape(shapeParent).GetComponent<Shape>();
            shape.CreateBlockList(listShapeData.Find(o => o.shapeID == shapeID));

            // Thêm shape vào danh sách shape trong game
            ShapeInGame.Add(shape);
        }
        
        // Tạo mới shape
        private GameObject GenerateShape(Transform parent)
        {
            var shapeGameObject = Instantiate(emptyShape, parent);
            var rectTransformShape = shapeGameObject.GetComponent<RectTransform>();
            rectTransformShape.anchoredPosition = Vector2.zero;
            rectTransformShape.localScale = Vector3.one * .5f;
            return shapeGameObject;
        }
    }
}
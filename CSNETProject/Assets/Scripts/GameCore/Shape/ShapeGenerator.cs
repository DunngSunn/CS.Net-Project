using System;
using System.Collections.Generic;
using DunnGSunn;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore
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
        
        public void GenerateShape()
        {
            CreateShapeBlockPool();
            FillShapeContainer();
        }
        
        public void CreateShapeBlockPool()
        {
            if (_shapeBlockPool == null)
            {
                _shapeBlockPool = new List<int>();
            }
            
            foreach (var shapeData in listShapeData)
            {
                var count = Random.Range(1, 4);
                for (var i = 0; i < count; i++)
                {
                    _shapeBlockPool.Add(shapeData.shapeID); 
                }
            }
        
            _shapeBlockPool.ShuffleList();
        }
        
        public void FillShapeContainer()
        {
            if (ShapeInGame == null)
            {
                ShapeInGame = new List<Shape>();
            }
        
            var isAllEmpty = ShapeInGame.Count <= 0;

            if (isAllEmpty)
            {
                foreach (var shapeContainer in shapeParents)
                {
                    AddShapeToContainer(shapeContainer);
                }
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void AddShapeToContainer(Transform shapeParent)
        {
            if (_shapeBlockPool == null || _shapeBlockPool.Count <= 0)
            {
                CreateShapeBlockPool();
            }

            var shapeID = _shapeBlockPool[0];
            _shapeBlockPool.RemoveAt(0);
            
            var shape = GenerateShape(shapeParent).GetComponent<Shape>();
            shape.CreateBlockList(listShapeData.Find(o => o.shapeID == shapeID));

            ShapeInGame.Add(shape);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
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
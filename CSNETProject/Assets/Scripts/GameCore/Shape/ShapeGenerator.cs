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

        public GameObject emptyShape;

        public Transform[] shapeParents;

        public List<ShapeData> listShapeData;
        
        public List<Shape> ShapeInGame { get; private set; }

        #endregion

        private void Start()
        {
            ShapeInGame = new List<Shape>();
            foreach (var shapeParent in shapeParents)
            {
                var randomData = listShapeData[Random.Range(0, listShapeData.Count)];
                var shapeControl = GenerateShape(shapeParent).GetComponent<Shape>();
                shapeControl.CreateBlockList(randomData);
                ShapeInGame.Add(shapeControl);
            }
        }
        
        private GameObject GenerateShape(Transform parent)
        {
            var shapeGameObject = Instantiate(emptyShape, parent);
            var rectTransformShape = shapeGameObject.GetComponent<RectTransform>();
            rectTransformShape.anchoredPosition = Vector2.zero;
            rectTransformShape.localScale = Vector3.one;
            return shapeGameObject;
        }
    }
}
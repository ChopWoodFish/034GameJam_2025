using System.Collections.Generic;
using UnityEngine;
using Util;

public class ShapeManager : MonoBehaviour
{
    private List<Shape> listShape = new List<Shape>();

    private void Start()
    {
        IntEventSystem.Register(GameEventEnum.GenerateShape, OnGenerateShape);
    }

    private void OnGenerateShape(object param)
    {
        if (param is Shape shape)
        {
            listShape.Add(shape);
            Debug.Log($"shape count: {listShape.Count}");
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ShapeManager : MonoBehaviour
{
    public int DebrisCount => _shapeGenerator.DebrisCount;
    
    [SerializeField] private ShapeGenerator _shapeGenerator;
    
    private List<Shape> listShape = new List<Shape>();
    
    
    private void Start()
    {
        IntEventSystem.Register(GameEventEnum.ClickShape, OnClickShape);
        IntEventSystem.Register(GameEventEnum.RecycleShape, OnRecycleShape);
    }

    public void GenerateOneShape()
    {
        var shape = _shapeGenerator.GenerateOneShape();
        shape.ShowFloating();
        listShape.Add(shape);
    }

    private void OnClickShape(object param)
    {
        if (param is Shape clickShape)
        {
            if (clickShape.Type == GameManager.Instance.CurrentShapeType)
            {
                DoRecycleShape(clickShape);
            }
            else
            {
                clickShape.DoBug();
                DoRecycleShape(clickShape);
            }
        }
    }

    private void OnRecycleShape(object param)
    {
        if (param is Shape toRecycleShape)
        {
            if (toRecycleShape.Type == GameManager.Instance.CurrentShapeType)
            {
                Debug.Log($"miss shape {toRecycleShape.Type}");
                toRecycleShape.DoBug();
            }
            DoRecycleShape(toRecycleShape);
        }
    }

    private void DoRecycleShape(Shape toRecycleShape)
    {
        if (listShape.Contains(toRecycleShape))
        {
            listShape.Remove(toRecycleShape);
            Destroy(toRecycleShape.gameObject);
        }
    }
}
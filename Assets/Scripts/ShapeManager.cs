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

    public void GenerateOneShape(int genType = -1)
    {
        var shape = _shapeGenerator.GenerateOneShape(genType);
        shape.ShowFloating();
        listShape.Add(shape);
    }

    private void OnClickShape(object param)
    {
        if (param is Shape clickShape)
        {
            if (clickShape.Type == GameManager.Instance.CurrentShapeType)
            {
                _shapeGenerator.GenerateCorrectParticle(clickShape);
                DoRecycleShape(clickShape);
                IntEventSystem.Send(GameEventEnum.CorrectClick, null);
            }
            else
            {
                clickShape.DoBug();
                DoRecycleShape(clickShape);
                IntEventSystem.Send(GameEventEnum.CheckStage, null);
            }
        }
    }

    private void OnRecycleShape(object param)
    {
        if (param is Shape toRecycleShape)
        {
            if (toRecycleShape.Type == GameManager.Instance.CurrentShapeType && !GameManager.Instance.IsInTuto)
            {
                toRecycleShape.DoBug();
                IntEventSystem.Send(GameEventEnum.CheckStage, null);
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
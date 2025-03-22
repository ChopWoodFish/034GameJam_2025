using System.Collections.Generic;
using UnityEngine;
using Util;

public class ShapeGenerator : MonoBehaviour
{
    [SerializeField] private List<Shape> listShapePrefab;
    [SerializeField] private Transform shapeParent;
    private UpdateComp _updateComp;

    private void Awake()
    {
        _updateComp = new UpdateComp();
    }

    private void Start()
    {
        _updateComp.ScheduleAction(Test, 0.5f, -1);
    }

    private void Test()
    {
        GenerateOneShape();
    }

    private void GenerateOneShape()
    {
        int genType = Random.Range(0, listShapePrefab.Count);
        Vector2 genPos = new Vector2(Random.Range(-15f, 15f), Random.Range(0f, 5f));
        Shape newShape = Instantiate(listShapePrefab[genType],  genPos, Quaternion.identity, shapeParent);
        IntEventSystem.Send(GameEventEnum.GenerateShape, newShape);
    }
}

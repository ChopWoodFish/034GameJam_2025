using System.Collections.Generic;
using UnityEngine;
using Util;

public class ShapeGenerator : MonoBehaviour
{
    [SerializeField] private List<Shape> listShapePrefab;
    [SerializeField] private Transform shapeParent;
    [SerializeField] private List<ShapeDebris> listShapeDebrisPrefab;
    [SerializeField] private Transform debrisParent;
    private UpdateComp _updateComp;
    
    
    private void Awake()
    {
        _updateComp = new UpdateComp();
        IntEventSystem.Register(GameEventEnum.GenerateShapeDebris, OnGenerateShapeDebris);
    }

    public Shape GenerateOneShape()
    {
        int genType = Random.Range(0, listShapePrefab.Count);
        Vector2 genPos = new Vector2(Random.Range(-13f, 13f), Random.Range(-9f, 9f));
        Shape newShape = Instantiate(listShapePrefab[genType],  genPos,
            Quaternion.Euler(0, 0, Random.Range(0f, 360f)) , shapeParent);
        // newShape.transform.localScale = Vector3.one;
        return newShape;
    }

    private void OnGenerateShapeDebris(object param)
    {
        if (param is Shape shape)
        {
            var listGenColor = GameManager.Instance.SO.GetDataSettings().listDebrisColor;
            var genPos = shape.transform.position;
            for (int i = 0; i < 10; i++)
            {
                int genType = Random.Range(0, listShapeDebrisPrefab.Count);
                int genColorIndex = Random.Range(0, listGenColor.Count);
                ShapeDebris shapeDebris = Instantiate(listShapeDebrisPrefab[genType],  
                    genPos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)),
                    Quaternion.Euler(0, 0, Random.Range(0f, 360f)) , debrisParent);
                var randomForce = new Vector2(Random.Range(15f, 30f), Random.Range(15f, 30f));
                if (Random.Range(0, 1f) < 0.5f)
                {
                    randomForce.x *= -1;
                }
                if (Random.Range(0, 1f) < 0.5f)
                {
                    randomForce.y *= -1;
                }
                shapeDebris.SetColor(listGenColor[genColorIndex]);
                shapeDebris.AddForce(randomForce);
            }
        }
    }
}

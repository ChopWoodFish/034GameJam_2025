using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class ShapeGenerator : MonoBehaviour
{
    public int DebrisCount => debrisParent.childCount;
    
    [SerializeField] private List<Shape> listShapePrefab;
    [SerializeField] private Transform shapeParent;
    [SerializeField] private List<ShapeDebris> listShapeDebrisPrefab;
    [SerializeField] private Transform debrisParent;
    [SerializeField] private GameObject debrisParticleGameObject;
    [SerializeField] private GameObject correctParticleGameObject;
    [SerializeField] private List<GameObject> listLiquidParticleGameObject;
    [SerializeField] private Transform particleParent;

    [SerializeField] private GameObject deadPixelPrefab;
    
    private UpdateComp _updateComp;
    
    
    private void Awake()
    {
        _updateComp = new UpdateComp();
        IntEventSystem.Register(GameEventEnum.GenerateShapeDebris, OnGenerateShapeDebris);
        IntEventSystem.Register(GameEventEnum.GenerateDeadPixel, OnGenerateDeadPixel);
        IntEventSystem.Register(GameEventEnum.ClearAllBug, OnClearAllBug);
    }

    private void OnClearAllBug(object _)
    {
        float delay = 0f;
        for (int i = 0; i < debrisParent.childCount; i++)
        {
            var child = debrisParent.GetChild(i);
            _updateComp.DelayAction(() =>
            {
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }, delay);
            delay += 0.1f;
        }
    }

    private void OnGenerateDeadPixel(object param)
    {
        if (param is Shape shape)
        {
            var genPos = shape.transform.position;
            var deadPixel = Instantiate(deadPixelPrefab, genPos, Quaternion.identity, debrisParent);
        }
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

    public void GenerateCorrectParticle(Shape shape)
    {
        var pos = shape.transform.position;
        var particle = Instantiate(correctParticleGameObject, pos, Quaternion.identity);
        _updateComp.DelayAction(() =>
        {
            Destroy(particle);
        }, 1f);
    }

    private void OnGenerateShapeDebris(object param)
    {
        if (param is Shape shape)
        {
            int minCount = GameManager.Instance.SO.GetDataSettings().MinGenerateDebrisCount;
            int maxCount = GameManager.Instance.SO.GetDataSettings().MaxGenerateDebrisCount;
            var listGenColor = GameManager.Instance.SO.GetDataSettings().listDebrisColor;
            var genPos = shape.transform.position;

            for (int i = 0; i < listLiquidParticleGameObject.Count; i++)
            {
                var liquidParticle = listLiquidParticleGameObject[i];
                if (!liquidParticle.activeSelf)
                {
                    liquidParticle.transform.position = genPos;
                    liquidParticle.SetActive(true);
                    break;
                }
            }
            var debrisParticle = Instantiate(debrisParticleGameObject, genPos, Quaternion.identity, debrisParent);
            
            _updateComp.DelayAction(() =>
            {
                Destroy(debrisParticle);
            }, 1f);
            
            for (int i = 0; i < Random.Range(minCount, maxCount + 1); i++)
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

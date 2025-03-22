using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSettings", menuName = "ScriptableObject/DataSettings")]
public class DataSettings : ScriptableObject
{
    public List<Color> listDebrisColor = new List<Color>();

    public float GenerateShapeDuration = 0.5f;
    public int GenerateShapeCount = 8;
    public float ShapeFadeDuration = 2f;

}
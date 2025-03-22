using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSettings", menuName = "ScriptableObject/DataSettings")]
public class DataSettings : ScriptableObject
{
    public List<Color> listDebrisColor = new List<Color>();

    public float BeatStateDelay = 1f;
    
    public float GenerateShapeDuration = 0.5f;
    public int GenerateShapeCount = 8;
    public float ShapeFadeDuration = 2f;
    public float GenerateStateDelay = 0.5f;

    public int MinGenerateDebrisCount = 8;
    public int MaxGenerateDebrisCount = 12;
    public int MaxDebrisCount = 50;

}
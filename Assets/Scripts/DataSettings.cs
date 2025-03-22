using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSettings", menuName = "ScriptableObject/DataSettings")]
public class DataSettings : ScriptableObject
{
    public List<Color> listDebrisColor = new List<Color>();
}
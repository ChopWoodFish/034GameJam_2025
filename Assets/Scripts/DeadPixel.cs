using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeadPixel : MonoBehaviour
{
    private int _pixelCount = 3;
    
    private void Start()
    {
        List<int> listShowPixelIndex = new List<int>();
        int childCount = transform.childCount;
        while (listShowPixelIndex.Count < _pixelCount)
        {
            int index = Random.Range(0, childCount);
            if (listShowPixelIndex.Contains(index))
            {
                continue;
            }
            listShowPixelIndex.Add(index);
        }

        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
            if (listShowPixelIndex.Contains(i))
            {
                child.gameObject.SetActive(true);
                child.GetComponent<Animator>().Play($"Flash{Random.Range(1, 7)}");
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Shape : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<Color> listColor = new List<Color>();
    [SerializeField] private SpriteRenderer srShape;

    private void Awake()
    {
        int colorIndex = Random.Range(0, listColor.Count);
        srShape.color = listColor[colorIndex];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"click {gameObject.name}");
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Shape : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<Color> listColor = new List<Color>();
    
    [SerializeField] private SpriteRenderer srShape;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Collider2D shapeCollider2D;
    [SerializeField] private Animator animator;
    
    
    private void Awake()
    {
        int colorIndex = Random.Range(0, listColor.Count);
        srShape.color = listColor[colorIndex];
    }

    private void Start()
    {
        // ShowFloating();
    }

    private void ShowFloating()
    {
        rigidBody2D.isKinematic = true;
        shapeCollider2D.enabled = false;
        animator.Play("Show");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"click {gameObject.name}");
    }
}
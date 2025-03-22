using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;
using Random = UnityEngine.Random;

public class Shape : MonoBehaviour, IPointerClickHandler
{
    public int Type => type;
    
    [SerializeField] private int type;
    [SerializeField] private List<Color> listColor = new List<Color>();
    
    [SerializeField] private SpriteRenderer srShape;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;

    private AnimationEventHelper _animHelper;
    private Collider2D _shapeCollider2D;
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        IntEventSystem.Send(GameEventEnum.ClickShape, this);
    }
    
    private void Awake()
    {
        _animHelper = GetComponent<AnimationEventHelper>();
        if (_animHelper != null)
        {
            _animHelper.OnAnim = OnAnim;
        }
        _shapeCollider2D = GetComponent<Collider2D>();

        int colorIndex = Random.Range(0, listColor.Count);
        srShape.color = listColor[colorIndex];
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void DoBug()
    {
        // if (type == 0)
        // {
            IntEventSystem.Send(GameEventEnum.GenerateShapeDebris, this);
        // }
    }

    public void AddForce(Vector2 force)
    {
        rigidBody2D.AddForce(force, ForceMode2D.Impulse);
    }

    public void ShowFloating()
    {
        rigidBody2D.isKinematic = true;  // 会导致点击失效
        // _shapeCollider2D.enabled = false;
        // rigidBody2D.gravityScale = 0;
        
        animator.Play("Show");
    }

    private void OnAnim(string animEventName)
    {
        if (animEventName == "End")
        {
            IntEventSystem.Send(GameEventEnum.RecycleShape, this);
        }
    }
}
using UnityEngine;

public class ShapeDebris : MonoBehaviour
{
    [SerializeField] private SpriteRenderer srShape;
    [SerializeField] private Rigidbody2D rigidBody2D;
    
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
    public void SetColor(Color color)
    {
        srShape.color = color;
    }
    
    public void AddForce(Vector2 force)
    {
        rigidBody2D.AddForce(force, ForceMode2D.Impulse);
    }
}
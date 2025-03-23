using System;
using UnityEngine;

public class ScreenSaver : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    
    private Rigidbody2D _rigidBody2d;
    
    
    private void Start()
    {
        _rigidBody2d = GetComponentInChildren<Rigidbody2D>();
        var direction = Vector2FromAngle(UnityEngine.Random.Range(0, 360)).normalized;
        _rigidBody2d.velocity = direction * speed;
    }
    
    private Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var direction = Vector2FromAngle(UnityEngine.Random.Range(0, 360)).normalized;
        _rigidBody2d.velocity = direction * speed;
    }
}
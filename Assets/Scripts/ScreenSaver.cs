using System;
using UnityEngine;

public class ScreenSaver : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Screen saver collision enter");
    }
}
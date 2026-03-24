using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.right);
        if (hit.collider == null)
            return;
        Debug.Log("HIT");
        
    }
}
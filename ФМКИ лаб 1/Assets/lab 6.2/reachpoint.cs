using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reachpoint : MonoBehaviour
{
    [SerializeField] private GameObject _cirlcle;
    
    
    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == _cirlcle)
        {
            _spriteRenderer.color = Color.green;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class spawnObstackles : MonoBehaviour
{

    [SerializeField] private GameObject _obstacle;
    [SerializeField] private float width, height;
    [SerializeField] private Transform parent;
    [SerializeField] private int minCount, maxCount;

    
    private void CreateObstacle()
    {
        float scaleX = Random.Range(1, 3);
        var position = new Vector3(
            Random.Range((-width + scaleX) / 2, (width - scaleX) / 2),
            Random.Range((-height + scaleX) / 2, (height - scaleX) / 2),
            0);

        Quaternion rotation = Quaternion.Euler(0,0,Random.value * 360);
        GameObject obj = Instantiate(_obstacle, position, rotation, parent);
        obj.transform.localScale = new Vector3(scaleX, 0.1f, 1);
        if (Random.value < .5f) 
        {
            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            spriteRenderer.color = Color.gray;
        }
    }

    void Start()
    {
        Respawn();
    }

    private void Spawn()
    {
        Debug.Log("Spawn");
        for (int i = 0; i < Random.Range(minCount, maxCount); i++)
        {
            CreateObstacle();
        }
    }

    public void Respawn()
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
        Spawn();
    }
    
}

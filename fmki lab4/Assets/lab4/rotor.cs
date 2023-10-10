using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotor : MonoBehaviour
{
    private float timeSpend;
    [SerializeField] private float speed = 1000; 
    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        timeSpend += Time.fixedDeltaTime;
        float x = speed * timeSpend;
        transform.rotation = Quaternion.Euler(x,0,0);
    }
}

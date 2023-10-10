using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private InputField vel1Field;
    [SerializeField] private InputField vel2Field;
    [SerializeField] private Button resetButton;
    [SerializeField] private Toggle m1greatest;
    [SerializeField] private Toggle m2greatest;
    [SerializeField] private Toggle massEqual;
    [SerializeField] private Toggle xPosToggle;
    [SerializeField] private Text _velocity1Stats;
    [SerializeField] private Text _velocity2Stats;
    
    
    [FormerlySerializedAs("_secondObject")]
    [Header("Other")]
    [SerializeField] private GameObject secondObject;

    [SerializeField] private float greatesMass;
    [SerializeField] private float lesserMass;
    

    private Rigidbody2D _rigidbody, _rigidbody1;
    private float _velocity1, _velocity2;
    private Vector3 startPosition1, startPosition2; 
    
    
    void Start()
    {
        startPosition1 = transform.position;
        startPosition2 = secondObject.transform.position;
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody1 = secondObject.GetComponent<Rigidbody2D>();
        
        vel1Field.onValueChanged.AddListener(value =>
        {
            float.TryParse(value, NumberStyles.Float, new NumberFormatInfo(), out _velocity1); 
        });
        
        vel2Field.onValueChanged.AddListener(value =>
        {
            float.TryParse(value, NumberStyles.Float, new NumberFormatInfo(), out _velocity2); 
        });
        
        resetButton.onClick.AddListener(onResetClick);
        m1greatest.onValueChanged.AddListener(m1GreaterCall);
        m2greatest.onValueChanged.AddListener(m2GreaterCall);
        massEqual.onValueChanged.AddListener(massEqualCall);
        xPosToggle.onValueChanged.AddListener(onXPosToggle);   
    }

    private void m1GreaterCall(bool arg0)
    {
        if (arg0)
        {
            m2greatest.isOn = false;
            massEqual.isOn = false;
     
            _rigidbody.mass = greatesMass;
            _rigidbody1.mass = lesserMass;
            setVelocity();
        }
    }

    private void m2GreaterCall(bool arg0)
    {
        if (arg0)
        {
            m1greatest.isOn = false;
            massEqual.isOn = false;
            
            _rigidbody.mass = lesserMass;
            _rigidbody1.mass = greatesMass;
            setVelocity();
        }
    }

    private void massEqualCall(bool arg0)
    {
        if (arg0)
        {
            m1greatest.isOn = false;
            m2greatest.isOn = false;
            
            _rigidbody.mass = greatesMass;
            _rigidbody1.mass = greatesMass;
            setVelocity();
        }
    }

    private void onXPosToggle(bool arg0)
    {
        transform.position += Vector3.up * (arg0 ? 0.1f : -0.1f);
        startPosition1 += Vector3.up * (arg0 ? 0.1f : -0.1f);
    }

    private void onResetClick()
    {
        transform.position = startPosition1;
        secondObject.transform.position = startPosition2;
        _rigidbody.velocity = _rigidbody1.velocity = Vector2.zero;

        m1greatest.isOn = false;
        m2greatest.isOn = false;
        massEqual.isOn = false;
    }

    private void setVelocity()
    {
        _rigidbody.velocity =  _velocity1 * Vector2.right;
        _rigidbody1.velocity = _velocity2 * Vector2.left;
    }

    void Update()
    {
        _velocity1Stats.text = $"Velocity1: {_rigidbody.velocity}";
        _velocity2Stats.text = $"Velocity2: {_rigidbody1.velocity}";
    }
}

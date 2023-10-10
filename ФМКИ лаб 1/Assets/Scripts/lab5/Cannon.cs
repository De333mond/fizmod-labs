using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    [SerializeField] private InputField _height;
    [SerializeField] private InputField _angle;
    [SerializeField] private InputField _velocityField;
    [SerializeField] private InputField accelerationField;
    
    [SerializeField] private Transform _column;
    [SerializeField] private Transform _tube;
    [SerializeField] private Transform _ball;

    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private CircleCollider2D _circleCollider;

    [SerializeField] private Button _shootBTN;
    [SerializeField] private Button _resetBTN;

    [SerializeField] private Toggle _isInstant;
    



    [Header("Stats")] [SerializeField] private Text _shootTime;
    [SerializeField] private Text _averageVel;
    [SerializeField] private Text _instantVel;
    [SerializeField] private Text _distance;
    [SerializeField] private Text _path;
    
    private Vector3 startPos;
    private Vector3 ball_statrtPos;

    private float ax = 0,ay = -9.8f;
    private float _velocity = 5;
    private float vx, vy;
    private float height;
    private float angle;
    private float _tubeAngleOffset = -18;
    private float time;
    private float path;
    private bool shooting;
    private float acceleration;
    private bool accHasBeenAdded = false;
    private bool isGrounded = false;
    private bool isInstant = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _height.onValueChanged.AddListener(value =>
        {
            height = float.Parse(value);
            onReset();
        });
        _angle.onValueChanged.AddListener((string value) => { angle = float.Parse(value); });
        _shootBTN.onClick.AddListener(onShoot);
        _resetBTN.onClick.AddListener(onReset);
        startPos = transform.position;
        ball_statrtPos = _ball.localPosition;
        _velocityField.onValueChanged.AddListener(value => { _velocity = float.Parse(value); });
        accelerationField.onValueChanged.AddListener(value => { acceleration = float.Parse(value);});   
        _isInstant.onValueChanged.AddListener(value => isInstant = value);
        _ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        // Cannon height
        transform.position = startPos + new Vector3(0, height, 0);
        _column.localScale = new Vector3(1, height, 1);

        //Tube angle
        _tube.transform.rotation = Quaternion.Euler(0, 0, angle + _tubeAngleOffset);

        if (shooting)
        {
            Move();
            updateStats();
        }
    }

    private void Move()
    {
        time += Time.fixedDeltaTime;
        // if (_ball.position.y < -4.15f)
        //     shooting = false;
        // else
        //     shooting = true;

        
        if (_isInstant.isOn && !accHasBeenAdded)
        {
            accHasBeenAdded = true;
            vx += acceleration * Mathf.Cos(angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
            vy += acceleration * Mathf.Sin(angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
        }
        if (!_isInstant.isOn)
        {
            vx += acceleration * Mathf.Cos(angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
            vy += acceleration * Mathf.Sin(angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
        }

        if (_circleCollider.IsTouching(_boxCollider))
            isGrounded = true;

        if (isGrounded)
            vy = 0;
        else
            vy += ay * Time.fixedDeltaTime;
        
        Vector3 nextPosition = new Vector3(vx * Time.fixedDeltaTime, vy * Time.fixedDeltaTime, 0);
        path += nextPosition.magnitude;
        _ball.position += nextPosition;
    }

    private void onShoot()
    {
        onReset();
        vx = _velocity * Mathf.Cos(angle * Mathf.Deg2Rad);
        vy = _velocity * Mathf.Sin(angle * Mathf.Deg2Rad);
        shooting = true;
        isGrounded = false;
        _ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

    }

    private void onReset()
    {
        _ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        _ball.localPosition = ball_statrtPos;
        vx = 0;
        vy = 0;
        time = 0;
        path = 0;
        updateStats();
        shooting = false;
        accHasBeenAdded = false;
        _ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void updateStats()
    {
        _shootTime.text = $"Time: {round(time)}";
        _path.text = $"Path: {round(path)}";
        _averageVel.text = $"AverageVel: {round(path / time)}";
        _instantVel.text = $"Velocity: {round(new Vector3(vx, vy, 0).magnitude)}";
        _distance.text = $"Distance: {-round(ball_statrtPos.x - _ball.localPosition.x)}";
    }

    private float round(float num)
    {
        return (float)(Mathf.Round(num * 100) * 0.01);
    }

    
}

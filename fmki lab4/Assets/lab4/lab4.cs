using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class lab4 : MonoBehaviour
{

    //UI
    [Header("UI-input")]
    [SerializeField] private InputField Radius;
    [SerializeField] private InputField Speed;
    [SerializeField] private InputField Height;
    [SerializeField] private InputField t;
    [SerializeField] private InputField t1;
    [SerializeField] private InputField A;
    [SerializeField] private InputField B;

    [Header("UI-stats")]
    [SerializeField] private Text Path;
    [SerializeField] private Text Coords;
    [SerializeField] private Text TimeSpend;
    [SerializeField] private Text AccelerationOut;

    //Buttons 
    [SerializeField] private Button StartBtn;
    [SerializeField] private Button ResetBtn;
    [SerializeField] private GameObject _camera;
    [SerializeField]  private float height = 6;


    private Vector3 startPosition, cameraStartPosition;

    // linear speed
    private float speed = 2;
    private float radius = 5;

    private float acc, time, time1, a, b;

    private float timeSpend, angle, yOffset, path;
    private bool Running = false;

    // Start is called before the first frame update
    void Awake()
    {
        yOffset = gameObject.transform.position.y;
        gameObject.transform.position = startPosition = new Vector3(radius, yOffset, 0);
        cameraStartPosition = _camera.transform.position;

        Speed.onValueChanged.AddListener(Speed_listener);
        Radius.onValueChanged.AddListener(Radius_listener);
        Height.onValueChanged.AddListener(Height_listener);
        t.onValueChanged.AddListener(T_listener);
        t1.onValueChanged.AddListener(T1_listener);
        A.onValueChanged.AddListener(A_listener);
        B.onValueChanged.AddListener(B_listener);

        StartBtn.onClick.AddListener(OnStartBtnClick);
        ResetBtn.onClick.AddListener(OnResetBtnClick);
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (Running)
        {

            timeSpend += Time.fixedDeltaTime;

            if (timeSpend >= time)
                Move();
        }
        UpdateStats();
    }

    private void Move()
    {
        if ((a != 0) || (b != 0))
            acc = a + b * timeSpend;

        speed += acc * Time.fixedDeltaTime;
        Debug.Log(acc);

        float angularVelocity = speed / radius;
        

        angle = angularVelocity * (timeSpend-time);
        float y = angle / 2 / Mathf.PI * height;
        

        Vector3 nextPosition = new Vector3(Mathf.Cos(angle) * radius, y + yOffset, Mathf.Sin(angle) * radius);
        // path += Vector3.Distance(transform.position, nextPosition);
        path += Vector3.Distance(gameObject.transform.position, nextPosition);
        // Moving 
        gameObject.transform.position = nextPosition;

        // Rotation 
        gameObject.transform.rotation = Quaternion.Euler(15, -90 - angle * Mathf.Rad2Deg, 25);

        // Camera moving
        _camera.transform.position += new Vector3(0, y / (timeSpend-time) * Time.fixedDeltaTime, 0);
    }

    private void UpdateStats()
    {
        if (timeSpend <= time1)
        {
            Vector3 p = gameObject.transform.position;
            Coords.text = $"Coords: {round(p.x)}, {round(p.y)}, {round(p.z)}";
        }

        TimeSpend.text = $"Time: {timeSpend}";
        if ((timeSpend-time) >= 0)
            Path.text = $"Path: {path}";
        
        AccelerationOut.text = $"Acceleration: {acc}";
    }


    private void Speed_listener(string s) { speed = float.Parse(s);  }
    private void Radius_listener(string s) { radius = float.Parse(s); }
    private void Height_listener(string s) { height = float.Parse(s);}
    private void T_listener(string s) { time = float.Parse(s);}
    private void T1_listener(string s) { time1 = float.Parse(s);}
    private void A_listener(string s) { a = float.Parse(s);}
    private void B_listener(string s) { b = float.Parse(s);}

    private void OnStartBtnClick()
    {
        Text StartButton_Text = StartBtn.GetComponentInChildren<Text>();
        Running = StartButton_Text.text == "Start";
        StartButton_Text.text = Running ? "Stop" : "Start";
    }

    private void OnResetBtnClick()
    {
        transform.position = startPosition;
        _camera.transform.position = cameraStartPosition;
        path = timeSpend = 0;
        speed = float.Parse(Speed.text);
        acc = float.Parse(A.text);
    }


    private float round(float value)
    {
        return (float)(Mathf.Round(value * 100.0f) * 0.01);
    }
}

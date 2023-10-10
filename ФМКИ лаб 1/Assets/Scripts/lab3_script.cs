using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lab3_script : MonoBehaviour
{

    [SerializeField] private InputField A, B, C, D, T1, T2, T3;

    [SerializeField] private Text TimeUI, VelocityUI, AccelerationUI, PathUI;

    [SerializeField] private Button StartButton, ResetButton;
    [SerializeField] private float a, b, c, d;
    [SerializeField] private float t1, t2, t3;


    private Vector3 StartPosition;
    private float timeSpend, path;

    float vx, vy, ax, ay, Sx, Sy;
    private bool Running = false;

    void Start()
    {
        A.onEndEdit.AddListener(A_onValueChanged);
        B.onValueChanged.AddListener(B_onValueChanged);
        C.onValueChanged.AddListener(C_onValueChanged);
        D.onValueChanged.AddListener(D_onValueChanged);
        T1.onValueChanged.AddListener(T1_onValueChanged);
        T2.onValueChanged.AddListener(T2_onValueChanged);
        T3.onValueChanged.AddListener(T3_onValueChanged);

        StartButton.onClick.AddListener(delegate { OnStartButtonClick(); });
        ResetButton.onClick.AddListener(delegate { OnResetButtonClick(); });

        StartPosition = gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        if (Running && timeSpend <= t2)
        {
            timeSpend += Time.fixedDeltaTime;
            if (timeSpend > t1)
                Move();
        }
        UpdateStats();
        rotate();
    }

    private void Move()
    {
        float dt = Time.fixedDeltaTime;

        ax = a + b * timeSpend;
        ay = c + d * timeSpend;

        vx += ax * dt;
        vy += ay * dt;


        Sx = vx * dt;
        Sy = vy * dt;
        path += (Mathf.Sqrt(Sx * Sx + Sy * Sy));


        gameObject.transform.position += new Vector3(vx * dt, vy * dt, 0);

    }

    private void UpdateStats()
    {
        if (timeSpend <= t3)
        {
            VelocityUI.text = $"Velocity: {vx}, {vy}";
            AccelerationUI.text = $"Acceleration: {ax}, {ay}";
        }

        if (timeSpend <= t2)
            TimeUI.text = $"Time: {timeSpend}";

        PathUI.text = $"Path: {path}";
    }

    private void rotate()
    {
        Vector3 normSpeed = Vector3.Normalize(new Vector3(vx, vy, 0));
        float angle = Mathf.Atan2(normSpeed.y, normSpeed.x) * Mathf.Rad2Deg;

        gameObject.transform.rotation = Quaternion.Lerp(
            gameObject.transform.rotation,
            Quaternion.Euler(0, 0, angle),
            Time.fixedDeltaTime * 10
        );

    }

    private void A_onValueChanged(string s) { a = float.Parse(s); }
    private void B_onValueChanged(string s) { b = float.Parse(s); }
    private void C_onValueChanged(string s) { c = float.Parse(s); }
    private void D_onValueChanged(string s) { d = float.Parse(s); }
    private void T1_onValueChanged(string s) { t1 = float.Parse(s); }
    private void T2_onValueChanged(string s) { t2 = float.Parse(s); }
    private void T3_onValueChanged(string s) { t3 = float.Parse(s); }

    private void OnStartButtonClick()
    {
        Text StartButton_Text = StartButton.GetComponentInChildren<Text>();
        Running = StartButton_Text.text == "Start";
        StartButton_Text.text = Running ? "Stop" : "Start";
    }

    private void OnResetButtonClick()
    {
        Debug.Log("click");
        gameObject.transform.position = StartPosition;
        timeSpend = vx = vy = ax = ay = path = 0;
    }

    private float round(float value)
    {
        return (float)(Mathf.Round(value * 100.0f) * 0.01);
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{

    [Header("UI-input")]
    [SerializeField] private InputField InputTimeField;
    [SerializeField] private InputField RadiusField;
    [SerializeField] private InputField PeriodField;
    [SerializeField] private Button StartStopButton;

    [Header("UI-stats")]
    [SerializeField] private Text rpt;
    [SerializeField] private Text Velocity;
    [SerializeField] private Text Angle;
    [SerializeField] private Text AngleAfterTime;
    [SerializeField] private Text Distance;
    [SerializeField] private Text LinearSpeed;
    [SerializeField] private Text Position;

    [Header("Input data")]
    [SerializeField] private Transform center;
    [SerializeField] private float Period = 1.0f;
    [SerializeField] private float radius = 10.0f;
    [SerializeField] private float inputTime = 3.0f;

    private Text StartStopButton_text;
    private float timeSpend = 0;
    private float angularVelocity;
    private float distance = 0;
    private float linearVelocity;
    private float angle = 0;

    const float PI = Mathf.PI;
    // Start is called before the first frame update
    void Start()
    {
        angularVelocity = 2 * PI / Period;
        StartStopButton_text = StartStopButton.GetComponentInChildren<Text>();

        RadiusField.onValueChanged.AddListener(value => radius = float.Parse(value));
        PeriodField.onValueChanged.AddListener(delegate {OnPeriodChange();});
        InputTimeField.onValueChanged.AddListener(delegate {OnInputTimeChange();});
        StartStopButton.onClick.AddListener(delegate {StartStopButtonClick();});

        RadiusField.text = radius.ToString();
        PeriodField.text = Period.ToString();
        InputTimeField.text = inputTime.ToString();

    }



    private void FixedUpdate()
    {
        if(StartStopButton_text.text == "Stop")
            Move();
        updateStats();

    }

    private void Move()
    {
        timeSpend += Time.fixedDeltaTime;
        angle = angularVelocity * timeSpend;
        gameObject.transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

        gameObject.transform.rotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg - 20, 0);

    }

    private void updateStats()
    {
        rpt.text = $"RPT: {1 / Period * inputTime}";

        angularVelocity = 2 * PI / Period;
        Velocity.text = $"Velocity: {round(angularVelocity)}";

        linearVelocity = 2 * PI * radius / Period;
        LinearSpeed.text = $"Linear Speed: {round(linearVelocity)}";

        AngleAfterTime.text = $"Angle after time: {round(angularVelocity * inputTime)}";

        Distance.text = $"Distance: {round(linearVelocity * inputTime)}";
        Angle.text = $"Angle: {round((angle * Mathf.Rad2Deg) % 360)}";
        Position.text = $"Position: {round(gameObject.transform.position.x)}, {round(gameObject.transform.position.z)}";
    }


    private float round(float value)
    {
        return (float)(Mathf.Round(value * 100.0f) * 0.01);
    }

    

    private void OnPeriodChange()
    {
        Period = float.Parse(PeriodField.text);
    }

    private void OnInputTimeChange()
    {
        inputTime = float.Parse(InputTimeField.text);
    }

    void StartStopButtonClick()
    {
        if(StartStopButton_text.text == "Start")
            StartStopButton_text.text = "Stop";
        else
            StartStopButton_text.text = "Start";
    }
}



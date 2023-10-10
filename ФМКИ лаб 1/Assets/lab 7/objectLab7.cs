using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class objectLab7 : MonoBehaviour
{
    [SerializeField] private TMP_InputField mass;
    [SerializeField] private TMP_InputField force;
    [SerializeField] private TMP_Text velocityStat;
    [SerializeField] private TMP_Text pathStat;
    
    [SerializeField] private GameObject obj;

    [SerializeField] private Transform pos1;
    [SerializeField] private Transform pos2;
    [SerializeField] private Transform pos3;

    [SerializeField] private Button pos1Btn;
    [SerializeField] private Button pos2Btn;
    [SerializeField] private Button pos3Btn;

    [SerializeField] private Button reset;
    [SerializeField] private Button start;


    private Rigidbody2D _rigidbody2D;
    private Vector3 startPosition;
    private Vector2 direction = Vector2.left;
    private Quaternion startRotation;
    private float _force;


    private void Start()
    {
        _rigidbody2D = obj.GetComponent<Rigidbody2D>();
        force.onValueChanged.AddListener(value => _force = float.Parse(value));
        mass.onValueChanged.AddListener(value => _rigidbody2D.mass = float.Parse(value));
        reset.onClick.AddListener(onResetClick);
        start.onClick.AddListener(onStartClick);
        startPosition = transform.position;
        startRotation = transform.rotation;

        pos1Btn.onClick.AddListener(delegate
        {
            var angle = Vector3.Angle(Vector3.right, pos1.transform.right);
            transform.rotation = Quaternion.Euler(0, 0, angle);

            transform.position = pos1.position;
            direction = Vector2.left;
        });

        pos2Btn.onClick.AddListener(delegate
        {
            var angle = Vector3.Angle(Vector3.right, pos2.transform.right);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            transform.position = pos2.position;
            direction = Vector2.left;
        });

        pos3Btn.onClick.AddListener(delegate
        {
            var angle = Vector3.Angle(Vector3.right, pos3.transform.right);
            transform.rotation = Quaternion.Euler(0, 0, angle);

            transform.position = pos3.position;
            direction = Vector2.right;
        });

        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    private void onStartClick()
    {
        var text = start.GetComponentInChildren<TMP_Text>();
        if (text.text == "Start")
        {
            text.text = "Stop";
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            text.text = "Start";
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void onResetClick()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        force.text = "0";
        direction = Vector2.left;
    }

    private void FixedUpdate()
    {
        Move();
        UpdateStats();
    }

    private void UpdateStats()
    {
        velocityStat.text = $"Velocity: {Mathf.Round(_rigidbody2D.velocity.magnitude * 100) * 0.01f}";
        var path = pos2.parent.transform.position.x - transform.position.x;
        if (path > 0 && direction == Vector2.left)
            pathStat.text = $"Path: {Mathf.Round(path * 100) * 0.01f}";
        else
            pathStat.text = "Path: 0";
    }

    private void Move()
    {
        _rigidbody2D.AddRelativeForce(direction);
    }
}
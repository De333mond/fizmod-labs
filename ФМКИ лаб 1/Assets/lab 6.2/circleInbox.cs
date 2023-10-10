using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class circleInbox : MonoBehaviour
{

    [Header("UI")] 
    [SerializeField] private Slider angleInput;
    [SerializeField] private TMP_Text angleText;
    [SerializeField] private TMP_InputField velocityInput;
    [SerializeField] private Button startStopBtn;
    
    
    
    [SerializeField] private float velocity = 20;
    [SerializeField] private float chanseGetAccel = 0.01f;
    [SerializeField] private float addedAccel = 5;
    [SerializeField] private spawnObstackles spawner;
    
    
    private Rigidbody2D _rigidBody;
    private float _angle;
    private Vector3 startPos;
    
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        angleInput.onValueChanged.AddListener(value =>
        {
            _angle = value;
            angleText.text = _angle.ToString();
        });
        velocityInput.onValueChanged.AddListener(value => { float.TryParse(value, out velocity);});
        startStopBtn.onClick.AddListener(onStartClick);

        startPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Random.value <= chanseGetAccel)
        {
            _rigidBody.velocity += _rigidBody.velocity.normalized * addedAccel;
        }
    }

    

    private void onStartClick()
    {
        var text = startStopBtn.GetComponentInChildren<TMP_Text>();
        

        if (text.text != "Start")
        {
            text.text = "Start";
            _rigidBody.velocity = Vector2.zero;
            transform.position = startPos;
            spawner.Respawn();
        }
        else
        {
            text.text = "Stop";
            _rigidBody.velocity = Quaternion.AngleAxis(_angle, Vector3.forward) * Vector3.right * velocity;
        }
    }
}

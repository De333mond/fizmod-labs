using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class rotatedPlatform : MonoBehaviour
{
    [SerializeField] private TMP_InputField _angleInput;
    
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject joinPlatform;
    [SerializeField] private Button reset;

    private GameObject _startPoint, _joinPoint;
    private float _initialAngle;
    private void Start()
    {
        _angleInput.onValueChanged.AddListener(delegate(string value) { Rotate(float.Parse(value)); });
        _startPoint = transform.GetChild(0).gameObject;
        _joinPoint = transform.GetChild(1).gameObject;
        _initialAngle = Vector3.Angle(Vector3.right, transform.right);
        reset.onClick.AddListener(() => Rotate(_initialAngle));
    }

    private void Rotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0,0,angle);
        obj.transform.rotation = Quaternion.Euler(0,0,angle);
        obj.transform.position = _startPoint.transform.position;
        joinPlatform.transform.position = _joinPoint.transform.position;
    }
}
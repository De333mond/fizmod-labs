using TMPro;
using UnityEngine;

public class frictionParser : MonoBehaviour
{
    [SerializeField] private TMP_InputField friction;
    private Collider2D _collider2D;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        friction.onValueChanged.AddListener(SetMaterial);
        if (friction.text != "")
            SetMaterial(friction.text);
    }

    private void SetMaterial(string value)
    {
        _collider2D.sharedMaterial = new PhysicsMaterial2D
        {
            friction = float.Parse(value), 
            name = "Friction: " + value
        };
    }
}
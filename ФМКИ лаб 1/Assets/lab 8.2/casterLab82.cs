using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class casterLab82 : MonoBehaviour
{
    [SerializeField] private int MaxStepCount = 50;
    [SerializeField] private GameObject target;

    [SerializeField] private Slider angle;
    [SerializeField] private TMP_Text angleText;

    [SerializeField] private Slider steps;
    [SerializeField] private TMP_Text stepsText;

    [SerializeField] private Button regenerateBtn;


    private LineRenderer _lineRenderer;
    private const float MaxRayDistance = 30f;

    private void Start()
    {
        SetupLineRenderer();
        Rotate(90);
        Redraw();

        angle.onValueChanged.AddListener(delegate(float value)
        {
            Rotate(value);
            angleText.text = $"Angle: {value}";
        });

        steps.onValueChanged.AddListener(delegate(float value)
        {
            MaxStepCount = (int)value;
            stepsText.text = $"Steps: {MaxStepCount}";
            Redraw();
        });

        regenerateBtn.onClick.AddListener(Redraw);
    }

    private void CalcLine4lense(Vector3 position, Vector3 direction, int step = 0)
    {
        if (step > MaxStepCount)
            return;


        var hit = Physics2D.Raycast(position, direction, MaxRayDistance, 1);

        if (hit.collider.gameObject != target)
        {
            target.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            target.GetComponent<SpriteRenderer>().color = Color.green;
            _lineRenderer.positionCount += 1;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);
            return;
        }

        Vector3 hitPos = hit.point;

        if (hit.point == Vector2.zero)
            hitPos = position + direction * MaxRayDistance;

        _lineRenderer.positionCount += 1;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hitPos);

        if (hit.point != Vector2.zero)
        {
            var mat = hit.collider.gameObject.GetComponent<MaterialClass>();
            if (mat && mat._materialData.lense)
            {
                var a = Vector2.SignedAngle(direction, hit.normal) * Mathf.Deg2Rad;
                var b = Mathf.Asin(Mathf.Sin(a) * 1f / mat._materialData.density);

                // Debug.DrawRay(hit.point, -hit.normal, Color.green);
                // Debug.DrawRay(hit.point, hit.normal, Color.red);

                var norm = -hit.normal;

                var x = norm.x * Mathf.Cos(b) - norm.y * Mathf.Sin(b);
                var y = norm.x * Mathf.Sin(b) + norm.y * Mathf.Cos(b);

                direction = new Vector2(x, y);

                hit = Physics2D.Raycast(hit.point + (Vector2)direction, -direction);
                Debug.DrawRay(hit.point + (Vector2)direction, -direction, Color.yellow);
                // Debug.DrawRay(hit.point, -hit.normal, Color.green);
                // Debug.DrawRay(hit.point, hit.normal, Color.red);

                a = Vector2.SignedAngle(-direction, hit.normal) * Mathf.Deg2Rad;
                b = Mathf.Asin(Mathf.Sin(a) * mat._materialData.density);

                if (float.IsNaN(b))
                    b = Mathf.Asin(Mathf.Sin(a) / mat._materialData.density);

                norm = hit.normal;

                x = norm.x * Mathf.Cos(b) - norm.y * Mathf.Sin(b);
                y = norm.x * Mathf.Sin(b) + norm.y * Mathf.Cos(b);

                direction = new Vector2(x, y);

                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);
            }
            else
            {
                direction = Vector3.Reflect(direction, hit.normal);
            }

            CalcLine4lense(hit.point + (Vector2)direction * 0.01f, direction, step + 1);
        }
    }

    private void Rotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Redraw();
    }

    private void Redraw()
    {
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);
        CalcLine4lense(transform.position, -transform.up);
    }


    private void SetupLineRenderer()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
        _lineRenderer.startColor = Color.cyan;
        _lineRenderer.endColor = Color.cyan;
    }
}
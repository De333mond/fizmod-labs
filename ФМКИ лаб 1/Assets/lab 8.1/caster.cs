    using UnityEngine;
    using UnityEngine.UI;
    using Color = UnityEngine.Color;


    public class caster : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Slider angleSlider;
        [SerializeField] private Text angleLabel;
        [SerializeField] private Toggle isLense;
        [SerializeField] private GameObject lense;
        [SerializeField] private Slider countSlider;


        private enum SceneState
        {
            Glasses,
            Lense
        }

        private SceneState _state;
        private LineRenderer _lineRenderer;
        private const float MaxRayDistance = 30f;
        private int _n;

        void Start()
        {
            SetupLineRenderer();
            angleSlider.onValueChanged.AddListener(Rotate);
            isLense.onValueChanged.AddListener(changeScene);
            
            countSlider.onValueChanged.AddListener(delegate(float value)
            {
                _n = (int)value;
                Redraw();
            });
            
            _state = SceneState.Glasses;
            Rotate(0);
        }

        private void Rotate(float angle)
        {
            angleLabel.text = $"Angle: {angle}";
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Redraw();
        }

        private void changeScene(bool value)
        {
            _container.SetActive(!value);
            lense.SetActive(value);
            _state = value ? SceneState.Lense : SceneState.Glasses;
            Redraw();
        }

        private void Redraw()
        {
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, transform.position);
            
            switch (_state)
            {
                case SceneState.Glasses:
                    CalcLine(transform.position, -transform.up);
                    break;
                case SceneState.Lense:
                    CalcLine4lense(transform.position, -transform.up);
                    break;
            }
        }

        private void CalcLine4lense(Vector3 position, Vector3 direction, int step=0)
        {
            var hit = Physics2D.Raycast(position, direction, MaxRayDistance, 1, step);
            
            Vector3 hitPos = hit.point;
            
            if (hit.point == Vector2.zero)
                hitPos = position + direction * MaxRayDistance;

            _lineRenderer.positionCount += 1;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hitPos);
            
            if (hit.point != Vector2.zero)
            {
                var mat = hit.collider.gameObject.GetComponent<MaterialClass>();
                if (mat._materialData.lense)
                {
                    float a = Vector2.SignedAngle(direction, hit.normal) * Mathf.Deg2Rad;
                    float b = Mathf.Asin((Mathf.Sin(a) * 1f / mat._materialData.density));
                    
                    // Debug.DrawRay(hit.point, -hit.normal, Color.green);
                    // Debug.DrawRay(hit.point, hit.normal, Color.red);
                    
                    Vector2 norm = -hit.normal;
                    
                    float x = norm.x * Mathf.Cos(b) - norm.y * Mathf.Sin(b); 
                    float y = norm.x * Mathf.Sin(b) + norm.y * Mathf.Cos(b); 
                    
                    direction = new Vector2(x, y);
                    
                    hit = Physics2D.Raycast(hit.point + ((Vector2)direction), -direction);

                    // Debug.DrawRay(hit.point, -hit.normal, Color.green);
                    // Debug.DrawRay(hit.point, hit.normal, Color.red);

                    a = Vector2.SignedAngle(-direction, hit.normal) * Mathf.Deg2Rad;
                    b = Mathf.Asin((Mathf.Sin(a) * mat._materialData.density));

                    norm = hit.normal;

                    x = norm.x * Mathf.Cos(b) - norm.y * Mathf.Sin(b); 
                    y = norm.x * Mathf.Sin(b) + norm.y * Mathf.Cos(b); 
                    
                    direction = new Vector2(x, y);
                    
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);
                }
                CalcLine4lense(hit.point + ((Vector2)direction * 0.01f), direction, step+1);    
            }
        }

        private MaterialClass prevMaterial;

        private void CalcLine(Vector3 position, Vector3 direction, int step = 0)
        {
            var hit = Physics2D.Raycast(position, direction, MaxRayDistance, 1, step);

            Vector3 hitPos = hit.point;

            if (hit.point == Vector2.zero)
                hitPos = position + direction * MaxRayDistance;

            _lineRenderer.positionCount += 1;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hitPos);

            if (hit.point != Vector2.zero)
            {
                var material = hit.collider.gameObject.GetComponent<MaterialClass>();
                if (step != 0 & step < _n + 2)
                {
                    float a = Vector2.SignedAngle(direction, hit.normal) * Mathf.Deg2Rad;
                    float b = Mathf.Asin(Mathf.Sin(a) *
                                         (prevMaterial._materialData.density / material._materialData.density));

                    Vector2 norm = -hit.normal;

                    float x = norm.x * Mathf.Cos(b) - norm.y * Mathf.Sin(b);
                    float y = norm.x * Mathf.Sin(b) + norm.y * Mathf.Cos(b);

                    direction = new Vector2(x, y);

                }

                prevMaterial = material;
                CalcLine(hit.point, direction, step + 1);
            }
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

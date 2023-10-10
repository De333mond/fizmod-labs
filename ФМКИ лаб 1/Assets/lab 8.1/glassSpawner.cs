using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class glassSpawner : MonoBehaviour
{
    [SerializeField] private Slider countSlider;
    [SerializeField] private Text countLabel;
    
    [FormerlySerializedAs("_width")] [SerializeField] private float _height;
    
    
    [SerializeField] private GameObject prefab;
    [SerializeField] private materialData[] _materials;

    
    
    private static Color colorFromRgb(int r, int g, int b)
    {
        return new Color(((float)r / 255), ((float)g / 255), ((float)b / 255));
    }
    
    // https://coolors.co/palette/264653-2a9d8f-e9c46a-f4a261-e76f51
    readonly Color[] _palette = {
        colorFromRgb(38, 70, 83),
        colorFromRgb(42, 157, 143),
        colorFromRgb(233, 196, 106),
        colorFromRgb(244, 162, 97),
        colorFromRgb(231, 111, 81),
    };
    
    void Start()
    {
        countSlider.onValueChanged.AddListener(Regenerate);    
        Regenerate(1);
    }

    private void Regenerate(float arg)
    {
        int count = ((int)arg);
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        countLabel.text = $"Count: {count}";
        for (int i = 0; i < count + 2; i++)
        {
            var obj = Instantiate(prefab, new Vector3(0, -_height*i, i), quaternion.identity, transform);
            var sprite = obj.GetComponent<SpriteRenderer>();
            var material = obj.GetComponent<MaterialClass>();
            
            obj.transform.localScale = new Vector3(20, _height, 0); 
            material._materialData = ScriptableObject.CreateInstance<materialData>();
            
            if (i == 0 | i == count + 1)
            {
                sprite.color = Color.clear;
                material._materialData.density = 1;
            }
            else
            {
                sprite.color = _palette[i-1];
                material._materialData.density = Random.RandomRange(1.1f, 2.5f);
            }

            material._materialData.lense = false;
        }

    }
}

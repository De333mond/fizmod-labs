using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace lab82
{
    
public class lensSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private GameObject mirror;

    [SerializeField] private Button regenerateBtn;
    
    
    [SerializeField] private int lensCount;
    [SerializeField] private int mirrorsCount;
    [SerializeField] private float height = 10;
    [SerializeField] private float width = 10;

    private const float LenseSize = 3f;
    
    void Start()
    {
        regenerateBtn.onClick.AddListener(Regenerate);
    }

    private void Regenerate()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        for (var i = 0; i < lensCount; i++)
        {
            var position = new Vector3(
                Random.Range((-width + LenseSize) / 2, (width - LenseSize) / 2),
                Random.Range((-height + LenseSize) / 2, (height - LenseSize) / 2), 0);

            var rotation = Quaternion.Euler(0, 0, Random.value * 360);

            Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], position, rotation, transform);
        }

        for (var i = 0; i < mirrorsCount; i++)
        {
            var position = new Vector3(
                Random.Range((-width + LenseSize) / 2, (width - LenseSize) / 2),
                Random.Range((-height + LenseSize) / 2, (height - LenseSize) / 2), 0);

            var rotation = Quaternion.Euler(0, 0, Random.value * 360);
            var scaleX = Random.Range(1,4);
            var newObj = Instantiate(mirror, position, rotation, transform);
            newObj.transform.localScale = new Vector3(scaleX, .2f,1);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
}
}

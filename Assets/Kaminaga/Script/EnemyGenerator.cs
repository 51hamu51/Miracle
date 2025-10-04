using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private GameObject _enemyPrefab;
    [SerializeField] private GameObject _spawnPositionLef;
    [SerializeField] private GameObject _spawnPositionRig;
    private Vector3 _spawnPosition;
    void Start()
    {
        _enemyPrefab = GameObject.Find("ScareEnemy");
        _spawnPosition = Vector3.zero;
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            _spawnPosition = _spawnPositionLef.transform.position;
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            _spawnPosition = _spawnPositionRig.transform.position;
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            Instantiate(_enemyPrefab, _spawnPosition, Quaternion.identity);
        }
    }
}

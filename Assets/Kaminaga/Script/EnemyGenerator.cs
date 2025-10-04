using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private GameObject _enemyPrefab;
    private Vector3 _spawnPosition;
    void Start()
    {
        _enemyPrefab = GameObject.Find("ScareEnemy");
        _spawnPosition = Vector3.zero;
    }

    void FixedUpdate()
    {
        _spawnPosition = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(_enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}

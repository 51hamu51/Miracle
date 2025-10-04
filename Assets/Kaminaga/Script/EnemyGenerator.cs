using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private GameObject _enemyPrefab;
    private GameObject _player;
    [SerializeField] private GameObject _spawnPositionLef;
    [SerializeField] private GameObject _spawnPositionRig;
    private Vector3 _spawnPositionCenter;
    private Vector3 _spawnArea;
    private Vector3 _spawnDirection;
    private int _spawnTimer;
    private int _spawnInterval;
    void Start()
    {
        _enemyPrefab = (GameObject)Resources.Load("ScareEnemy");
        _player = GameObject.Find("Player");
        _spawnPositionCenter = Vector3.zero;
        _spawnArea = Vector3.zero;
        _spawnTimer = 0;
        _spawnInterval = 300;
    }

    void FixedUpdate()
    {
        _spawnTimer++;
        if (_spawnTimer >= _spawnInterval)
        {
            SetSpawnPoint();
            _spawnArea = _spawnPositionCenter + new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));
            Instantiate(_enemyPrefab, _spawnArea, Quaternion.identity);
            _spawnTimer = 0;
        }
        _spawnDirection = (_spawnArea - _spawnPositionCenter).normalized;

        // 生成の位置までの方向を取得
        Debug.DrawRay(_spawnPositionCenter, (_spawnArea - _spawnPositionCenter), Color.red);

        // 生成の位置までエフェクトが移動した後に生成



    }

    void SetSpawnPoint()
    {
        if ((_player.transform.position - _spawnPositionLef.transform.position).magnitude > (_player.transform.position - _spawnPositionRig.transform.position).magnitude)
        {
            _spawnPositionCenter = _spawnPositionLef.transform.position;
        }
        else
        {
            _spawnPositionCenter = _spawnPositionRig.transform.position;
        }
    }
}

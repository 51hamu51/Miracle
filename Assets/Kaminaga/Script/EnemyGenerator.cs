using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private GameObject _enemyPrefab;
    private GameObject _player;
    [SerializeField] private GameObject _spawnPositionLef;
    [SerializeField] private GameObject _spawnPositionRig;
    private GameObject _spawnEffect;
    private Vector3 _spawnPositionCenter;
    private Vector3 _spawnArea;
    private Vector3 _spawnDirection;
    private int _spawnTimer;
    private int _spawnInterval;
    private bool _isSpawning;
    void Start()
    {
        _enemyPrefab = (GameObject)Resources.Load("ScareEnemy");
        _player = GameObject.Find("Player");
        _spawnEffect = GameObject.Find("SpawnEffect");
        _spawnPositionCenter = Vector3.zero;
        _spawnArea = Vector3.zero;
        _spawnDirection = Vector3.zero;
        _spawnTimer = 0;
        _spawnInterval = 300;
        _isSpawning = false;
    }

    void FixedUpdate()
    {
        if (!_isSpawning)
        {
            _spawnTimer++;
        }

        if (_spawnTimer >= _spawnInterval)
        {
            SetSpawnPoint();
            _isSpawning = true;
            _spawnArea = _spawnPositionCenter + new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));
            _spawnTimer = 0;
        }

        
        // 生成の位置までの方向を取得
        _spawnDirection = (_spawnArea - _spawnPositionCenter) / 100;
        Debug.DrawRay(_spawnPositionCenter, (_spawnArea - _spawnPositionCenter), Color.red);
        if(_isSpawning)
        {
            _spawnEffect.transform.position += _spawnDirection;
        }
        else
        {
            _spawnEffect.transform.position = _spawnPositionCenter;
        }

        // 生成の位置までエフェクトが移動した後に生成
        if (_isSpawning && (_spawnArea - _spawnEffect.transform.position).magnitude < 0.1f)
        {
            Instantiate(_enemyPrefab, _spawnArea, Quaternion.identity);
            _isSpawning = false;
        }



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
        _spawnEffect.transform.position = _spawnPositionCenter;
    }
}

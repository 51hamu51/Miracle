using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private GameObject _enemyPrefab;
    private GameObject _player;
    [SerializeField] private GameObject _spawnPositionLef; // 生成位置の座標取得用
    [SerializeField] private GameObject _spawnPositionRig; // 生成位置の座標取得用    
    private GameObject _spawnEffect; // 生成位置を追うエフェクト用
    private Vector3 _spawnPositionCenter;
    private Vector3 _spawnArea;
    private Vector3 _spawnDirection;
    private int _spawnTimer;
    private const int _spawnInterval = 300;
    private const int _effectMoveDuration = 100; // エフェクトが生成位置に移動する時間
    private const int _effectStopDuration = 25; // エフェクトが止まる時間
    private int _effectStopTimer;
    private bool _isSpawning;
    void Start()
    {
        _enemyPrefab = (GameObject)Resources.Load("ScareEnemy");
        _player = GameObject.Find("Player");
        _spawnEffect = GameObject.Find("SpawnEffect");
        _spawnPositionCenter = _spawnPositionLef.transform.position;
        _spawnArea = Vector3.zero;
        _spawnDirection = Vector3.zero;
        _spawnTimer = 0;
        _effectStopTimer = 0;
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
        _spawnDirection = (_spawnArea - _spawnPositionCenter) / _effectMoveDuration;
        if(_isSpawning)
        {
            Debug.DrawRay(_spawnPositionCenter, (_spawnArea - _spawnPositionCenter), Color.red);
            _spawnEffect.transform.position += _spawnDirection;
        }
        else
        {
            // エフェクトを中心に戻す
            _spawnEffect.transform.position = _spawnPositionCenter;
        }

        // 生成の位置までエフェクトが移動した後に生成
        if (_isSpawning && (_spawnArea - _spawnEffect.transform.position).magnitude < 0.1f)
        {
            _spawnEffect.transform.position = _spawnArea; // 誤差をなくすために直接代入
            _effectStopTimer++;
            if(_effectStopTimer > _effectStopDuration) // エフェクトが止まってから少し待つ
            {
                _effectStopTimer = 0;
                Instantiate(_enemyPrefab, _spawnArea, Quaternion.identity);
                _isSpawning = false;
            }
        }



    }

    void SetSpawnPoint()
    {
        // プレイヤーから一番近い生成位置を中心に設定
        if ((_player.transform.position - _spawnPositionLef.transform.position).magnitude > (_player.transform.position - _spawnPositionRig.transform.position).magnitude)
        {
            _spawnPositionCenter = _spawnPositionRig.transform.position;
        }
        else
        {
            _spawnPositionCenter = _spawnPositionLef.transform.position;
        }
        _spawnEffect.transform.position = _spawnPositionCenter;
    }
}

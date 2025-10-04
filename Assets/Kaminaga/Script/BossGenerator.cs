using UnityEngine;

public enum BossGeneratorState
{
    Easy,
    Normal,
    Hard,
}

public class BossGenerator : MonoBehaviour
{
    private GameObject _bossPrefab;
    [SerializeField] private GameObject _spawnPosition; // 生成位置の座標取得用
    private GameObject _spawnEffect; // 生成位置を追うエフェクト用
    private Vector3 _spawnArea;
    private Vector3 _spawnDirection;
    private int _spawnTimer;
    private int _spawnInterval;
    private int _effectMoveDuration; // エフェクトが生成位置に移動する時間
    private int _effectStopDuration; // エフェクトが止まる時間
    private int _effectStopTimer;
    private bool _isSpawning;
    private BossGeneratorState _currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bossPrefab = (GameObject)Resources.Load("BossEnemy");
        _spawnEffect = GameObject.Find("SpawnEffectBoss");
        _spawnArea = Vector3.zero;
        _spawnDirection = Vector3.zero;
        _spawnTimer = 0;
        _spawnInterval = 600;
        _effectMoveDuration = 100; // エフェクトが生成位置に移動する時間
        _effectStopDuration = 25;
        _effectStopTimer = 0;
        _isSpawning = false;
        _currentState = BossGeneratorState.Easy;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 難易度に応じて生成間隔を変更
        switch (_currentState)
        {
            case BossGeneratorState.Easy:
                _spawnInterval = 600;
                break;
            case BossGeneratorState.Normal:
                _spawnInterval = 400;
                break;
            case BossGeneratorState.Hard:
                _spawnInterval = 200;
                break;
        }
        if (!_isSpawning)
        {
            _spawnTimer++;
        }
        if (_spawnTimer >= _spawnInterval)
        {
            _isSpawning = true;
            _spawnArea = _spawnPosition.transform.position + new Vector3(0.0f,0.0f,Random.Range(-2.0f,-5.0f));
            _spawnTimer = 0;
        }
        // 生成の位置までの方向を取得
        _spawnDirection = (_spawnArea - _spawnPosition.transform.position) / _effectMoveDuration;
        if (_isSpawning)
        {
            _spawnEffect.transform.position += _spawnDirection;
        }
        else
        {
            _spawnEffect.transform.position = _spawnPosition.transform.position;
        }

        if (_isSpawning && (_spawnEffect.transform.position - _spawnArea).magnitude < 0.1f)
        {
            _spawnEffect.transform.position = _spawnArea;
            _effectStopTimer++;
            if (_effectStopTimer >= _effectStopDuration)
            {
                Instantiate(_bossPrefab, _spawnArea, Quaternion.identity);
                _isSpawning = false;
                _effectStopTimer = 0;
            }
        }
    }
}

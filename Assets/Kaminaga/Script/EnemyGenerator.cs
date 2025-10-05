using UnityEngine;

public enum EnemyGeneratorState
{
    Easy,
    Normal,
    Hard,
}
public class EnemyGenerator : MonoBehaviour
{
    private GameObject _hopperPrefab;
    private GameObject _cowPrefab;
    private GameObject _elephantPrefab;
    private GameObject _player;
    [SerializeField] private GameObject _spawnPositionLef; // 生成位置の座標取得用
    [SerializeField] private GameObject _spawnPositionRig; // 生成位置の座標取得用    
    private GameObject _spawnEffect; // 生成位置を追うエフェクト用
    private Vector3 _spawnPositionCenter;
    private Vector3 _spawnArea;
    private Vector3 _spawnDirection;
    private int _spawnTimer;
    private const int kEasyInterval = 300; // ・ｽ・ｽﾕ度・ｽC・ｽ[・ｽW・ｽ[・ｽﾌ撰ｿｽ・ｽ・ｽ・ｽﾔ隔
    private const int kNormalInterval = 200; // ・ｽ・ｽﾕ度・ｽm・ｽ[・ｽ}・ｽ・ｽ・ｽﾌ撰ｿｽ・ｽ・ｽ・ｽﾔ隔
    private const int kHardInterval = 100; // ・ｽ・ｽﾕ度・ｽn・ｽ[・ｽh・ｽﾌ撰ｿｽ・ｽ・ｽ・ｽﾔ隔
    private const int kEffectMoveDuration = 100; // ・ｽG・ｽt・ｽF・ｽN・ｽg・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽﾊ置・ｽﾉ移難ｿｽ・ｽ・ｽ・ｽ骼橸ｿｽ・ｽ
    private const int kEffectStopDuration = 25; // ・ｽG・ｽt・ｽF・ｽN・ｽg・ｽ・ｽ・ｽ~・ｽﾜる時・ｽ・ｽ
    private int _effectStopTimer;
    private bool _isSpawning;
    private bool _isSpawnRight;
    private EnemyGeneratorState _currentState;
    private int _spawnInterval;
    void Start()
    {
        _hopperPrefab = (GameObject)Resources.Load("Enemy_Hopper");
        _cowPrefab = (GameObject)Resources.Load("Enemy_Cow");
        _elephantPrefab = (GameObject)Resources.Load("Enemy_Elephant");
        _player = GameObject.FindWithTag("Player");
        _spawnEffect = GameObject.Find("SpawnEffect");
        _spawnPositionCenter = _spawnPositionLef.transform.position;
        _spawnArea = Vector3.zero;
        _spawnDirection = Vector3.zero;
        _spawnTimer = 0;
        _effectStopTimer = 0;
        _isSpawning = false;
        _isSpawnRight = false;
        _currentState = EnemyGeneratorState.Easy;
        _spawnInterval = kEasyInterval;
    }

    void FixedUpdate()
    {
        // ・ｽ・ｽﾕ度・ｽﾉ会ｿｽ・ｽ・ｽ・ｽﾄ撰ｿｽ・ｽ・ｽ・ｽﾔ隔・ｽ・ｽﾏ更
        switch (_currentState)
        {
            case EnemyGeneratorState.Easy:
                _spawnInterval = kEasyInterval;
                break;
            case EnemyGeneratorState.Normal:
                _spawnInterval = kNormalInterval;
                break;
            case EnemyGeneratorState.Hard:
                _spawnInterval = kHardInterval;
                break;
        }
        if (!_isSpawning)
        {
            _spawnTimer++;
        }

        if (_spawnTimer >= _spawnInterval)
        {
            SetSpawnPoint();
            _isSpawning = true;
            if (_isSpawnRight)
            {
                _spawnArea = _spawnPositionCenter + new Vector3(Random.Range(-5.5f, -2.0f), 0.0f, Random.Range(-4.0f, 7.0f));
            }
            else
            {
                _spawnArea = _spawnPositionCenter + new Vector3(Random.Range(2.0f, 5.5f), 0.0f, Random.Range(-4.0f, 7.0f));
            }
            _spawnTimer = 0;
        }


        // ・ｽ・ｽ・ｽ・ｽ・ｽﾌ位置・ｽﾜでの包ｿｽ・ｽ・ｽ・ｽ・ｽ・ｽ謫ｾ
        _spawnDirection = (_spawnArea - _spawnPositionCenter) / kEffectMoveDuration;
        if (_isSpawning)
        {
            _spawnEffect.transform.position += _spawnDirection;
        }
        else
        {
            // ・ｽG・ｽt・ｽF・ｽN・ｽg・ｽ・S・ｽﾉ戻ゑｿｽ
            _spawnEffect.transform.position = _spawnPositionCenter;
        }

        // ・ｽ・ｽ・ｽ・ｽ・ｽﾌ位置・ｽﾜでエ・ｽt・ｽF・ｽN・ｽg・ｽ・ｽ・ｽﾚ難ｿｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽﾉ撰ｿｽ・ｽ・ｽ
        if (_isSpawning && (_spawnArea - _spawnEffect.transform.position).magnitude < 0.1f)
        {
            _spawnEffect.transform.position = _spawnArea; // ・ｽ・ｷ・ｽ・ｽ・ｽﾈゑｿｽ・ｽ・ｽ・ｽ・ｽ・ｽﾟに抵ｿｽ・ｽﾚ托ｿｽ・ｽ
            _effectStopTimer++;
            if (_effectStopTimer > kEffectStopDuration) // ・ｽG・ｽt・ｽF・ｽN・ｽg・ｽ・ｽ・ｽ~・ｽﾜゑｿｽ・ｽﾄゑｿｽ・ｽ迴ｭ・ｽ・ｽ・ｽﾒゑｿｽ
            {
                _effectStopTimer = 0;
                Instantiate(_hopperPrefab, _spawnArea, Quaternion.identity);
                _isSpawning = false;
            }
        }



    }

    void SetSpawnPoint()
    {
        // ・ｽv・ｽ・ｽ・ｽC・ｽ・ｽ・ｽ[・ｽ・ｽ・ｽ・ｽ・ｽﾔ近ゑｿｽ・ｽ・ｽ・ｽ・ｽ・ｽﾊ置・ｽ・S・ｽﾉ設抵ｿｽ
        if ((_player.transform.position - _spawnPositionLef.transform.position).magnitude > (_player.transform.position - _spawnPositionRig.transform.position).magnitude)
        {
            _spawnPositionCenter = _spawnPositionRig.transform.position;
            _isSpawnRight = true;
        }
        else
        {
            _spawnPositionCenter = _spawnPositionLef.transform.position;
            _isSpawnRight = false;
        }
        _spawnEffect.transform.position = _spawnPositionCenter;
    }
}

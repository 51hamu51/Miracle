using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    public PlayerManager playerManager;
    void Start()
    {

    }
    void Update()
    {
        if (playerManager.IsEating || playerManager.IsRotating)
        {
            return;
        }
        // プレイヤーの後方に一定距離で追従
        Vector3 targetPos = playerTransform.position;
        targetPos.y += 4;
        targetPos.z -= 6;
        transform.position = targetPos;
    }
}

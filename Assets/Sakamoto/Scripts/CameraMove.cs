using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform playerTransform;

    private PlayerManager playerManager;
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
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

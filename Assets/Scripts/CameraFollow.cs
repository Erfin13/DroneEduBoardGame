using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 目標人物
    public float smoothSpeed = 0.125f; // 相機移動的平滑速度
    public Vector3 offset; // 相機與目標之間的偏移

    // 你可以在Inspector視窗調整這些參數，來限制相機的範圍
    public float minX = -10f; // X軸最小範圍
    public float maxX = 10f;  // X軸最大範圍
    public float minZ = -10f; // Z軸最小範圍
    public float maxZ = 10f;  // Z軸最大範圍

    void Start()
    {
        // 設定偏移，視場景需要做調整
        offset = new Vector3(0, 5, -10); // 可以調整相機的位置
    }

    void LateUpdate()
    {
        // 計算新的相機位置
        float desiredX = Mathf.Clamp(target.position.x + offset.x, minX, maxX);  // 限制X軸的移動範圍
        float desiredY = target.position.y + offset.y;
        float desiredZ = target.position.z + offset.z;

        Vector3 desiredPosition = new Vector3(desiredX, desiredY, desiredZ);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 更新相機位置
        transform.position = smoothPosition;

        // 讓相機始終面對目標
        transform.LookAt(target);
    }
}

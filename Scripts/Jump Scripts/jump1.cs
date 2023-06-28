using UnityEngine;

public class jump1 : MonoBehaviour
{
    public Transform leftController;
    public Transform rightController;
    public float trackingScale = 1f;

    void Update()
    {
        // VIVEトラッカーからプレイヤーの位置情報を取得
        Vector3 playerPosition = (leftController.position + rightController.position) * 0.5f * trackingScale;

        // キャラクターの位置に反映
        transform.position = playerPosition;
    }
}
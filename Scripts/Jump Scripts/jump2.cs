using UnityEngine;

public class jump2 : MonoBehaviour
{
    public GameObject avatar;
    public Transform leftController;
    public Transform rightController;
    public float jumpForce = 5f;
    public float jumpThreshold = 0.2f;

    private bool isJumping = false;
    private bool isStretching = false;

    void Update()
    {
        // 左右のコントローラーの位置情報を取得
        Vector3 leftControllerPos = leftController.position;
        Vector3 rightControllerPos = rightController.position;

        // コントローラーが一定の位置より下にある場合にジャンプ
        if ((leftControllerPos.y < jumpThreshold) && (rightControllerPos.y < jumpThreshold))
        {
            if (!isStretching)
            {
                isJumping = true;
                isStretching = true;
            }
        }
        else
        {
            isStretching = false;
        }
    }

    void FixedUpdate()
    {
        // ジャンプ処理
        if (isJumping)
        {
            // ジャンプ方向を計算（斜め前方向）
            Vector3 jumpDirection = (transform.forward + transform.up).normalized;

            // キャラクターにジャンプ力を与える
            Rigidbody avatarRigidbody = avatar.GetComponent<Rigidbody>();
            avatarRigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

            isJumping = false;
        }
    }
}
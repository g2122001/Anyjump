using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class Buttonjump : MonoBehaviour
{
    public GameObject avatar;
    public float jumpForce;

    public bool isJumping;
    public SteamVR_Input_Sources GrabPinch;


    void Update()
    {
        // トリガーボタンが引かれたらジャンプ
        if (SteamVR_Input.GetStateDown("A", GrabPinch))
        {
            isJumping = true;
            FixedUpdate();
            Debug.Log("A");
        }
    }

    void FixedUpdate()
    {
        // ジャンプ処理
        if (isJumping)
        {
            // ジャンプ方向を計算（斜め前方向）
            Vector3 jumpDirection = (transform.forward + transform.up).normalized;

            // アバターにジャンプ力を与える
            Rigidbody avatarRigidbody = avatar.GetComponent<Rigidbody>();
            avatarRigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

            isJumping = false;
        }
    }
}

//public class Buttonjump : MonoBehaviour
//{
//    public GameObject avatar;
//    public float jumpForce;

//    public SteamVR_Action_Boolean triggerAction;
//    public Rigidbody avatarRigidbody;
//    public SteamVR_Input_Sources GrabPinch;

//    private void Start()
//    {
//        // トリガーボタンのアクションを取得
//        triggerAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "Trigger");

//        // アバターのRigidbodyを取得
//        avatarRigidbody = avatar.GetComponent<Rigidbody>();
//    }

//    private void Update()
//    {
//        // トリガーボタンが押されたらジャンプ
//        if (SteamVR_Input.GetStateDown("A", GrabPinch))
//        {
//            Debug.Log("A");
//            Jump();
//        }
//    }

//    private void Jump()
//    {
//        // アバターにジャンプ力を与える
//        avatarRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//    }
//}
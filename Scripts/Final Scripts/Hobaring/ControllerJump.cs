using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;
using System.Collections;
using UnityEngine.XR;

public class ControllerJump : MonoBehaviour
{
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;
    private Vector3 HMDPosition;
    private Vector3 Controller;
    public float jumpThreshold; // ジャンプの速度閾値
    public float groundDistanceThreshold; // 地面からの距離の閾値
    public string groundTag = "Ground"; // 地面のタグ
    enum State { Neutral, Jump, Slow };
    State state = State.Neutral;
    private bool isJumping = false;//Jump中かを判断する
    private Vector3 initialPosition; // 初期位置
    public float jumpForce;
    Vector3 Ground = new Vector3(0f, 0f, 0f);
    private Vector3 GroundTranceform;//地面の位置情報



    private void Start()
    {
        // 初期位置を保存
        initialPosition = transform.position;

    }

    private void Update()
    {


        HMDPosition = InputTracking.GetLocalPosition(XRNode.Head);

        Controller = tracker1.GetLocalPosition(SteamVR_Input_Sources.RightHand);


        float distance = Vector3.Distance(Controller, HMDPosition);
        Debug.Log("Distance =" + distance);

        //Groundの位置を取得
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRigの取得
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);
        //Debug.Log("groundDistance=" +groundDistance);


        // 地面からの距離が閾値を超えた場合の処理
        if (groundDistance > groundDistanceThreshold)
        {
            //Debug.Log("条件クリア！");
            switch (state)
            {
                case State.Neutral:
                    // コントローラーの速度の大きさが閾値を超えた場合にジャンプ
                    if (distance > jumpThreshold)
                    {
                        Debug.Log("ControllerJump_Jump");
                        state = State.Jump;
                        break;
                    }
                    break;

                case State.Jump:
                    Jump();
                    state = State.Slow;
                    Debug.Log("ControllerJumo_Slow");
                    break;

                case State.Slow:
                    //コントローラーの速度が一定より低くなったらNeutralに遷移
                    if (distance < 0.3)
                    {
                        state = State.Neutral;
                        Debug.Log("ControllerJump_Neutral");
                        break;
                    }
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // 地面に接触したらジャンプと浮遊をリセット
        if ((other.gameObject.tag == "Ground") && (isJumping = true))
        {
            ResetPosition();
            ResetForce();
        }
    }

    private void ResetForce()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        // 力をゼロにする
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Jump()
    {
        isJumping = true;
        // ジャンプ方向を計算（斜め前方向）
        Vector3 jumpDirection = (transform.forward + transform.up).normalized;
        // ジャンプ処理を実装する
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        }
    }
    private void ResetPosition()
    {
        // 位置を初期位置にリセットする
        transform.position = initialPosition;
        isJumping = false;
    }
}
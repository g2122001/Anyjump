using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;
using System.Collections;
using UnityEngine.XR;
public class ControllerDown : MonoBehaviour
{
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;
    public SteamVR_Input_Sources handType; // コントローラーの種類（左手または右手）
    private Vector3 HMDPosition;
    private Vector3 Controller;
    public float jumpThreshold; // コントローラーの速度閾値
    public float groundDistanceThreshold; // 地面からの距離の閾値
    public string groundTag = "Ground"; // 地面のタグ
    enum State { Neutral, Down, Slow };
    State state = State.Neutral;
    private bool isJumping = false;//Jump中かを判断する
    private Vector3 initialPosition; // 初期位置
    public float diveForce;

    private void Start()
    {
        // 初期位置を保存
        initialPosition = transform.position;
    }


    private void Update()
    {
     


        // HMDの位置を取得
        //Vector3 hmdPosition = hmdPose.GetLocalPosition(SteamVR_Input_Sources.Head);
        HMDPosition = InputTracking.GetLocalPosition(XRNode.Head);


        Controller = tracker1.GetLocalPosition(SteamVR_Input_Sources.RightHand);

        float distance = Vector3.Distance(Controller, HMDPosition);
        //Debug.Log("Distance =" + distance);

        //Groundの位置を取得
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRigの取得
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);

        // 地面からの距離が閾値を超えた場合の処理
        if (groundDistance > groundDistanceThreshold)
        {

            switch (state)
            {
                case State.Neutral:
                    // コントローラーの速度の大きさが閾値を超えた場合に急降下
                    if (distance > jumpThreshold)
                    {
                        Debug.Log("Down遷移完了");
                        state = State.Down;
                        break;
                    }
                    break;
                case State.Down:
                    Down();
                    state = State.Slow;
                    Debug.Log("Slow遷移完了");
                    break;

                case State.Slow:
                         //コントローラーの速度が一定より低くなったらNeutralに遷移
                         if (distance < jumpThreshold) 
                    {
                        state = State.Neutral;
                             Debug.Log("Neutral遷移完了");
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

    private void Down()
    {
        isJumping = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // 水平方向の速度を保持
        rb.AddForce(Vector3.down * diveForce, ForceMode.Impulse);
    }

    private void ResetPosition()
    {
        // 位置を初期位置にリセットする
        transform.position = initialPosition;
        isJumping = false;
    }
}
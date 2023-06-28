using System;
using UnityEngine;
using Valve.VR;
using System.Collections;
using Valve.VR.InteractionSystem;
using System.Threading;
using UnityEngine.Events;
using UnityEngine.XR;


public class BaseJump : MonoBehaviour
{
    //public SteamVR_TrackedObject footTracker;
    //public SteamVR_TrackedObject trackedObject;
    //private VelocityEstimator velocityEstimator;
    private Vector3 Tracker1Posision;
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;

    //public SteamVR_Action_Pose hmdPose;
    private Vector3 HMDPosition;
    public float groundDistanceThreshold; // 地面からの距離の閾値
    public string groundTag = "Ground"; // 地面のタグ
    private Transform hmdTransform; // HMDのトランスフォーム
    public float jumpThreshold; // ジャンプ判定の距離のしきい値
    public float jumpForce; // ジャンプ力
    enum State { Neutral, Save, Jump };
    State state = State.Neutral;
    private bool isJumping = false;//Jump中かを判断する
    private Vector3 initialPosition; // 初期位置
    Vector3 Ground = new Vector3(0f, 0f, 0f);
    private Vector3 GroundTranceform;//地面の位置情報


    private void Start()
    {
        // 初期位置を保存
        initialPosition = transform.position;
    }

    private void Update()
    {
        // 膝のトラッカーの位置を取得
        //Vector3 footPosition = footTracker.transform.position;
        Tracker1Posision = tracker1.GetLocalPosition(SteamVR_Input_Sources.LeftFoot);

        // HMDの位置を取得
        //Vector3 hmdPosition = hmdPose.GetLocalPosition(SteamVR_Input_Sources.Head);
        HMDPosition = InputTracking.GetLocalPosition(XRNode.Head);

        // 距離を計算
        float distance = Vector3.Distance(Tracker1Posision, HMDPosition);//この距離に応じてjumpThresholdなどを変えていく

        //試しで距離をPrintしてみる
        //Debug.Log("Distance =" +distance);

        //Vector3 HMDS= velocityEstimator.GetVelocityEstimate();

        //Groundの位置を取得
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRigの取得
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);
        //Debug.Log("groudndistanse="+groundDistance);


        // ジャンプする前なのでDistanseが閾値より低かったら
        if (groundDistance < groundDistanceThreshold)
        {
            switch (state)
            {
                case State.Neutral:
                    if (distance < jumpThreshold)
                    {
                        state = State.Save;
                        Debug.Log("Save遷移完了");
                        break;
                    }
                    break;
                case State.Save:
                    if (distance >= 0.9)//数値要検討
                    {
                        state = State.Jump;
                        Debug.Log("Jump遷移完了");
                        break;
                    }
                    break;
                //else if (HMDS.magnitude < 1)
                //{
                //    state = State.Neutral;
                //    Debug.Log("Neutral遷移完了");
                //    break;
                //}
                //break;
                case State.Jump:
                    Jump();
                    state = State.Neutral;
                    Debug.Log("Neutral遷移完了");
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

    //private float GetGroundHeight()
    //{
    //    // Groundというタグのオブジェクトを探し、その位置を返す
    //    GameObject groundObject = GameObject.FindGameObjectWithTag(groundTag);
    //    if (groundObject != null)
    //    {
    //        return groundObject.transform.position.y;
    //    }
    //    else
    //    {
    //        return 0f; // 地面が見つからない場合は0を返す（デフォルトの高さ）
    //    }
    //}

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
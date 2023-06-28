using System;
using UnityEngine;
using Valve.VR;
using System.Collections;
using Valve.VR.InteractionSystem;
using System.Threading;
using UnityEngine.Events;
using UnityEngine.XR;
public class HMDWithTrackerDown : MonoBehaviour
{
    //public SteamVR_TrackedObject footTracker;
    //public SteamVR_TrackedObject trackedObject;
    //private VelocityEstimator velocityEstimator;
    private Vector3 Tracker1Posision;
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;

    //public SteamVR_Action_Pose hmdPose;
    private Vector3 HMDPosition;
    public float groundDistanceThreshold; // 地面からの距離の閾値
    public float jumpThreshold; // ジャンプ判定の距離のしきい値
    public float diveForce;        // 急降下力
    private Rigidbody rb;
    public string groundTag = "Ground"; // 地面のタグ
    enum State { Neutral, Down, Back };
    State state = State.Neutral;
    private bool isJumping = false;//Jump中かを判断する
    private Vector3 initialPosition; // 初期位置

    private void Start()
    {
        // 初期位置を保存
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
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
        Debug.Log("Distance =" +distance);

        //Groundの位置を取得
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRigの取得
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);

        //groundDistanceをprintしてみる
        //Debug.Log("groundDistance=" +groundDistance);


        // 地面からの距離が閾値を超えた場合の処理
        if (groundDistance > groundDistanceThreshold)
        {
            switch (state)
            {
                case State.Neutral:
                    if (distance < jumpThreshold)
                    {
                        state = State.Down;
                        Debug.Log("Down遷移完了");
                        break;
                    }
                    break;

                case State.Down:
                    Down();
                    state = State.Back;
                    Debug.Log("Back遷移完了");
                    break;

                case State.Back:
                    if (distance >= 0.9)//数値要検討
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
            Rigidbody rb = GetComponent<Rigidbody>();
            isJumping = true;
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

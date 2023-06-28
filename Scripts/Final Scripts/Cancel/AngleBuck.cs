using System;
using UnityEngine;
using Valve.VR;
using System.Collections;
using Valve.VR.InteractionSystem;
using System.Threading;
using UnityEngine.Events;
using UnityEngine.XR;

public class AngleBuck : MonoBehaviour
{
    private Vector3 HMDPosition;
    public float groundDistanceThreshold; // 地面からの距離の閾値
    public string groundTag = "Ground"; // 地面のタグ
    private Transform hmdTransform; // HMDのトランスフォーム
    enum State { Neutral, Buck };
    State state = State.Neutral;
    private bool isJumping = false;//Jump中かを判断する
    private Vector3 initialPosition; // 初期位置
    private Rigidbody rb;
    public float forceMagnitude = 5f; // 進行方向と逆の力の大きさ
    public float thresholdAngle; // 進行方向と逆に進む条件となる角度の閾値

    public SteamVR_Action_Pose poseAction = SteamVR_Input.GetAction<SteamVR_Action_Pose>("Pose");

    private void Start()
    {
        // HMDのトランスフォームを取得
        rb = GetComponent<Rigidbody>();
        // 初期位置を保存
        initialPosition = transform.position;


    }

    private void Update()
    {

        // ヘッドマウントディスプレイの向きを取得
        Transform targetTransform = GameObject.Find("Camera").transform;

        Quaternion rotation = targetTransform.rotation;


        //Debug.Log("Rotation"+rotation);

        // 上方向ベクトルを取得
        Vector3 upDirection = Vector3.up;

        // HMDの上方向ベクトルを取得
        Vector3 hmdUpDirection =rotation * upDirection;

        // 上向き角度の計算
        float angle = Vector3.Angle(hmdUpDirection, upDirection);


        //角度を取ってみる
        Debug.Log("angle=" + angle);

        //Groundの位置を取得
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRigの取得
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);


        // 地面からの距離が閾値を超えていたら
        if (groundDistance > groundDistanceThreshold)
        {
            switch (state)
            {
                case State.Neutral:
                    //angleが閾値を超えていたら
                    if (angle > thresholdAngle)
                    {
                        state = State.Buck;
                        Debug.Log("Buck遷移完了");
                        break;
                    }
                    break;

                case State.Buck:
                    Buck();
                    state = State.Neutral;
                    Debug.Log("Neutral遷移完了");
                    break;

                    //case State.Slow:
                    //   if (distance >= 0.5)//数値要検討
                    //   {
                    //     state = State.Neutral;
                    //       Debug.Log("Neutral遷移完了");
                    //       break;
                    //   }
                    //   break;
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

    private void ResetPosition()
        {
            // 位置を初期位置にリセットする
            transform.position = initialPosition;
            isJumping = false;
        }

        private void Buck()
        {
            isJumping = true;
            // 進行方向と逆の方向に力を加える
            rb.AddForce(-transform.forward * forceMagnitude, ForceMode.Impulse);
        }
    }
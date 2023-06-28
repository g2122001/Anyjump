using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using System;

public class ButtonDown : MonoBehaviour
{
    public GameObject avatar;
    public float diveForce;        // 急降下力
    private Rigidbody rb;
    private Transform hmdTransform; // HMDのトランスフォーム
    public bool isJumping = false;//Jump中かの判断
    public SteamVR_Input_Sources GrabPinch;
    private Vector3 initialPosition; // 初期位置
    public float groundDistanceThreshold; // 地面からの距離の閾値
    Vector3 Ground = new Vector3(0f, 0f, 0f);
    private Vector3 GroundTranceform;//地面の位置情報

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 初期位置を保存
        initialPosition = transform.position;
    }

    void Update()
    {

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

            // トリガーボタンが引かれたら急降下する
            if (SteamVR_Input.GetStateDown("A", GrabPinch))
            {
                Down();
                //Debug.Log("Down");
            }
        }
    }

        //ジャンプした後地面に着いたら位置をリセット
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
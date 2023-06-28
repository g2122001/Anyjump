using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using System;

public class ButtonJump : MonoBehaviour
{
    public GameObject avatar;
    public float jumpForce;

    public bool isJumping = false;
    public SteamVR_Input_Sources GrabPinch;
    private Vector3 initialPosition; // 初期位置

    private void Start()
    {
        // 初期位置を保存
        initialPosition = transform.position;
    }

    void Update()
    {
        // トリガーボタンが引かれたらジャンプ
        if (SteamVR_Input.GetStateDown("A", GrabPinch))
        {
            Jump();
            //Debug.Log("A");
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
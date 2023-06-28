using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using System;

public class ButtonBuck : MonoBehaviour
{
    public GameObject avatar;
    public bool isJumping = false;
    public SteamVR_Input_Sources GrabPinch;
    private Vector3 initialPosition; // 初期位置
    public float forceMagnitude = 5f; // 進行方向と逆の力の大きさ
    private Rigidbody rb;

    private void Start()
    {
        // 初期位置を保存
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // トリガーボタンが引かれている間後退する
        if (SteamVR_Input.GetState("A", GrabPinch))
        {
            Buck();
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

    private void Buck()
    {
        isJumping = true;
        // 進行方向と逆の方向に力を加える
        rb.AddForce(-transform.forward * forceMagnitude, ForceMode.Impulse);
    }

    private void ResetPosition()
    {
        // 位置を初期位置にリセットする
        transform.position = initialPosition;
        isJumping = false;
    }
}

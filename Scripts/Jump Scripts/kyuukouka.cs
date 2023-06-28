using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kyuukouka : MonoBehaviour
{
    public float jumpForce = 5f;         // ジャンプ力
    public float diveForce = 10f;        // 急降下力

    private Rigidbody rb;
    private bool isJumping = false;
    private bool isDiving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isDiving)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }

        // 急降下開始
        if (Input.GetKeyDown(KeyCode.Z) && isJumping && !isDiving)
        {
            isDiving = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // 水平方向の速度を保持
            rb.AddForce(Vector3.down * diveForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 地面に接触したらジャンプ状態と急降下状態をリセット
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            isDiving = false;
        }
    }
}
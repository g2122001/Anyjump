using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class fuwafuwa : MonoBehaviour
{
    public float jumpForce = 5f;         // ジャンプ力
    public float floatForce = 2f;        // 浮遊力
    public float maxFloatDuration = 2f;  // 浮遊時間の最大値

    private Rigidbody rb;
    private bool isJumping = false;
    private bool isFloating = false;
    private float floatTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isFloating)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }

        // 浮遊
        if (isJumping && !isFloating)
        {
            floatTimer += Time.deltaTime;
            if (floatTimer >= maxFloatDuration)
            {
                isFloating = true;
            }
        }

        // 浮遊中の落下
        if (isFloating)
        {
            rb.AddForce(Vector3.down * floatForce, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 地面に接触したらジャンプと浮遊をリセット
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            isFloating = false;
            floatTimer = 0f;
        }
    }
}
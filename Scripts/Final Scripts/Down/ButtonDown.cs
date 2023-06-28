using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using System;

public class ButtonDown : MonoBehaviour
{
    public GameObject avatar;
    public float diveForce;        // �}�~����
    private Rigidbody rb;
    private Transform hmdTransform; // HMD�̃g�����X�t�H�[��
    public bool isJumping = false;//Jump�����̔��f
    public SteamVR_Input_Sources GrabPinch;
    private Vector3 initialPosition; // �����ʒu
    public float groundDistanceThreshold; // �n�ʂ���̋�����臒l
    Vector3 Ground = new Vector3(0f, 0f, 0f);
    private Vector3 GroundTranceform;//�n�ʂ̈ʒu���

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // �����ʒu��ۑ�
        initialPosition = transform.position;
    }

    void Update()
    {

        //Ground�̈ʒu���擾
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRig�̎擾
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);

        //groundDistance��print���Ă݂�
        //Debug.Log("groundDistance=" +groundDistance);

        // �n�ʂ���̋�����臒l�𒴂����ꍇ�̏���
        if (groundDistance > groundDistanceThreshold)
        {

            // �g���K�[�{�^���������ꂽ��}�~������
            if (SteamVR_Input.GetStateDown("A", GrabPinch))
            {
                Down();
                //Debug.Log("Down");
            }
        }
    }

        //�W�����v������n�ʂɒ�������ʒu�����Z�b�g
        private void OnCollisionEnter(Collision other)
        {
            // �n�ʂɐڐG������W�����v�ƕ��V�����Z�b�g
            if ((other.gameObject.tag == "Ground") && (isJumping = true))
            {
                ResetPosition();
                ResetForce();
            }
        }

    private void ResetForce()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        // �͂��[���ɂ���
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Down()
        {
            isJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // ���������̑��x��ێ�
            rb.AddForce(Vector3.down * diveForce, ForceMode.Impulse);
        }


        private void ResetPosition()
        {
            // �ʒu�������ʒu�Ƀ��Z�b�g����
            transform.position = initialPosition;
            isJumping = false;
        }
    }
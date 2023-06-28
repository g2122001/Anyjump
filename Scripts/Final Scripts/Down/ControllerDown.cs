using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;
using System.Collections;
using UnityEngine.XR;
public class ControllerDown : MonoBehaviour
{
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;
    public SteamVR_Input_Sources handType; // �R���g���[���[�̎�ށi����܂��͉E��j
    private Vector3 HMDPosition;
    private Vector3 Controller;
    public float jumpThreshold; // �R���g���[���[�̑��x臒l
    public float groundDistanceThreshold; // �n�ʂ���̋�����臒l
    public string groundTag = "Ground"; // �n�ʂ̃^�O
    enum State { Neutral, Down, Slow };
    State state = State.Neutral;
    private bool isJumping = false;//Jump�����𔻒f����
    private Vector3 initialPosition; // �����ʒu
    public float diveForce;

    private void Start()
    {
        // �����ʒu��ۑ�
        initialPosition = transform.position;
    }


    private void Update()
    {
     


        // HMD�̈ʒu���擾
        //Vector3 hmdPosition = hmdPose.GetLocalPosition(SteamVR_Input_Sources.Head);
        HMDPosition = InputTracking.GetLocalPosition(XRNode.Head);


        Controller = tracker1.GetLocalPosition(SteamVR_Input_Sources.RightHand);

        float distance = Vector3.Distance(Controller, HMDPosition);
        //Debug.Log("Distance =" + distance);

        //Ground�̈ʒu���擾
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRig�̎擾
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);

        // �n�ʂ���̋�����臒l�𒴂����ꍇ�̏���
        if (groundDistance > groundDistanceThreshold)
        {

            switch (state)
            {
                case State.Neutral:
                    // �R���g���[���[�̑��x�̑傫����臒l�𒴂����ꍇ�ɋ}�~��
                    if (distance > jumpThreshold)
                    {
                        Debug.Log("Down�J�ڊ���");
                        state = State.Down;
                        break;
                    }
                    break;
                case State.Down:
                    Down();
                    state = State.Slow;
                    Debug.Log("Slow�J�ڊ���");
                    break;

                case State.Slow:
                         //�R���g���[���[�̑��x�������Ⴍ�Ȃ�����Neutral�ɑJ��
                         if (distance < jumpThreshold) 
                    {
                        state = State.Neutral;
                             Debug.Log("Neutral�J�ڊ���");
                        break;
                    }
                    break;
            }
        }
    }

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
        Rigidbody rb = GetComponent<Rigidbody>();
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
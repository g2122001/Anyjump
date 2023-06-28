using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class Buttonjump : MonoBehaviour
{
    public GameObject avatar;
    public float jumpForce;

    public bool isJumping;
    public SteamVR_Input_Sources GrabPinch;


    void Update()
    {
        // �g���K�[�{�^���������ꂽ��W�����v
        if (SteamVR_Input.GetStateDown("A", GrabPinch))
        {
            isJumping = true;
            FixedUpdate();
            Debug.Log("A");
        }
    }

    void FixedUpdate()
    {
        // �W�����v����
        if (isJumping)
        {
            // �W�����v�������v�Z�i�΂ߑO�����j
            Vector3 jumpDirection = (transform.forward + transform.up).normalized;

            // �A�o�^�[�ɃW�����v�͂�^����
            Rigidbody avatarRigidbody = avatar.GetComponent<Rigidbody>();
            avatarRigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

            isJumping = false;
        }
    }
}

//public class Buttonjump : MonoBehaviour
//{
//    public GameObject avatar;
//    public float jumpForce;

//    public SteamVR_Action_Boolean triggerAction;
//    public Rigidbody avatarRigidbody;
//    public SteamVR_Input_Sources GrabPinch;

//    private void Start()
//    {
//        // �g���K�[�{�^���̃A�N�V�������擾
//        triggerAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "Trigger");

//        // �A�o�^�[��Rigidbody���擾
//        avatarRigidbody = avatar.GetComponent<Rigidbody>();
//    }

//    private void Update()
//    {
//        // �g���K�[�{�^���������ꂽ��W�����v
//        if (SteamVR_Input.GetStateDown("A", GrabPinch))
//        {
//            Debug.Log("A");
//            Jump();
//        }
//    }

//    private void Jump()
//    {
//        // �A�o�^�[�ɃW�����v�͂�^����
//        avatarRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//    }
//}
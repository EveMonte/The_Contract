using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject od; 
    public float moveSpeed = 3.0f;
    private CharacterController _charController;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.81f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;
    private float _vertSpeed;
    private ControllerColliderHit _contact;
    private Animator _animator;
    private bool _canDoubleJump = true;
    public GameObject follow;
    AnimatorClipInfo[] m_CurrentClipInfo;
    private bool _gotDoubleJump = false;

    // Start is called before the first frame update
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _vertSpeed = minFall;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        float vertInput = Input.GetAxisRaw("Vertical");
        if(vertInput != 0)
        {
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

        }
        if(movement != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movement);
        _animator.SetFloat("Speed", movement.sqrMagnitude);
        m_CurrentClipInfo = this._animator.GetCurrentAnimatorClipInfo(0);
        bool hitGround = false;
        RaycastHit hit;
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
            float check =
            (_charController.height + _charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }
        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _animator.SetBool("Jumping", true);
                _vertSpeed = jumpSpeed;
                _canDoubleJump = true;

            }
            else
            {
                _vertSpeed = minFall;
                _animator.SetBool("Jumping", false);
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (_canDoubleJump && _gotDoubleJump)
                {
                    _vertSpeed = jumpSpeed;
                    _canDoubleJump = false;

                }
            }

            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }
            if (_contact != null)
            {
                _animator.SetBool("Jumping", true);
            }
            if (_charController.isGrounded)
            {
                _animator.SetBool("Jumping", false);

                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * moveSpeed;
                }
                else
                {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }
        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "OffDoubleJump")
        {
            _gotDoubleJump = false;
        }
        else if (other.gameObject.tag == "OnDoubleJump")
        {
            _gotDoubleJump = true;
            if(od !=null)
                Destroy(od);

        }

    }
}

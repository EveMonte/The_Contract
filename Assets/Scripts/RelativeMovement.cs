using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
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
    [HideInInspector]
    public bool _gotDoubleJump = true;
    private ControllerColliderHit _hit;
    [SerializeField] private float _duration;
    private Vector3 movement = Vector3.zero;
    [SerializeField] private float _height;
    private bool isJumping;
    private Coroutine _jumping;
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _vertSpeed = minFall;
        _animator = GetComponent<Animator>();
        ActivateDoubleJump();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        float vertInput = Input.GetAxisRaw("Horizontal");
        if (vertInput != 0)
        {
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

        }
        if (movement != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movement);
        _animator.SetFloat("Speed", movement.sqrMagnitude);
        m_CurrentClipInfo = this._animator.GetCurrentAnimatorClipInfo(0);
        bool hitGround = false;
        RaycastHit hit;
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check =
            (_charController.height + _charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }
        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _animator.SetBool("Jumping", true);
                _canDoubleJump = true;
                PlayAnimations(transform);
                if (_contact != null)
                {
                    if (_contact.gameObject.tag == "disposable")
                    {
                        _contact.gameObject.SendMessage("Break", jumpSpeed);
                    }
                    _contact.gameObject.SendMessage("MoveUp", SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                _vertSpeed = minFall;
                //_animator.SetBool("Jumping", false);
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (_canDoubleJump && _gotDoubleJump)
                {
                    _animator.SetBool("Jumping", false);
                    StopCoroutine(_jumping);
                    PlayAnimations(transform);
                    _animator.SetBool("DoubleJump", true);
                    Debug.Log("Input " + _animator.GetBool("Jumping") + " " + _animator.GetFloat("Speed"));

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
            }
            if (_charController.isGrounded)
            {
                _animator.SetBool("DoubleJump", false);

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
        //if(!isJumping)
            movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
        if (hit.gameObject.tag == "disposable")
        {
            hit.gameObject.SendMessage("Ready");

        }
        else if (hit.gameObject.tag == "fake")
        {
            hit.gameObject.SendMessage("Break", jumpSpeed);
        }
        hit.gameObject.SendMessage("MoveDown", SendMessageOptions.DontRequireReceiver);
    }

    private void ActivateDoubleJump()
    {
        _gotDoubleJump = true;
    }
    [SerializeField] private AnimationCurve _yAnimation;

    [ContextMenu("Play Animations")]
    public void PlayAnimations(Transform transform)
    {
        _jumping = StartCoroutine(AnimationByTime(transform));
    }

    public IEnumerator AnimationByTime(Transform _transform)
    {
        isJumping = true;
        Vector3 startPosition = transform.position;
        float expiredSeconds = 0;
        float progress = 0;
        while (progress < 1)
        {
            expiredSeconds += Time.deltaTime;
            progress = expiredSeconds / _duration;
            movement.y = startPosition.y + _height * _yAnimation.Evaluate(progress) - transform.position.y;
            _charController.Move(movement);
            Debug.Log("Coroutine " + movement.y + " " + _animator.GetBool("Jumping") + " height " + _height * _yAnimation.Evaluate(progress) + " start position " + startPosition.y + " current position " + transform.position.y + " progress " + progress);
            
            yield return null;
        }
        _animator.SetBool("Jumping", false);
        _animator.SetBool("DoubleJump", false);
        isJumping = false;
    }

}

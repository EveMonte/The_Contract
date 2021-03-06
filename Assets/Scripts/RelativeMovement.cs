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
    private bool isntFake = true;
    private Animator _animator;
    private bool _canDoubleJump = false;
    private bool _gotDoubleJump = false;
    [SerializeField] private float _duration;
    private Vector3 movement = Vector3.zero;
    [SerializeField] private float _height;
    private bool isJumping;
    private Coroutine _jumping;
    private float vertSpeed;
    [SerializeField] private AnimationCurve _yAnimation;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _vertSpeed = minFall;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "OUT")
            Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Vector3 movement = Vector3.zero;
        float vertInput = Input.GetAxis("Horizontal");
        if (vertInput != 0 && (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "OUT" || (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "OUT" && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)))
        {
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);
        }
        if (movement != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movement);

        _animator.SetFloat("Speed", movement.sqrMagnitude);
        RaycastHit hit;

        if (Physics.SphereCast(transform.position + _charController.center, _charController.radius / 1.5f, Vector3.down, out hit, (_charController.height + _charController.radius) / 2f) && isntFake) // character is on the ground
        {
            _animator.SetBool("Jumping", false);
            _animator.SetBool("Falling", false);
            if (Input.GetButtonDown("Jump") && (!isJumping || _animator.GetBool("Falling")) && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "OUT")
            {
                _canDoubleJump = true;
                vertSpeed = jumpSpeed;

                _jumping = StartCoroutine(AnimationByTime(transform)); 
                _animator.SetBool("Jumping", true);

                if (_contact != null)
                {
                    if (_contact.gameObject.tag == "disposable")
                    {
                        _contact.gameObject.SendMessage("Break", jumpSpeed, SendMessageOptions.DontRequireReceiver);
                    }
                    _contact.gameObject.SendMessage("MoveUp", SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                _vertSpeed = minFall;
            }
        }
        else
        {
            if(_charController.transform.position.y < -6)
            {
                _animator.SetBool("Falling", false);
                _animator.SetBool("Jumping", false);
                _animator.SetBool("DoubleJump", false);

                gameObject.SendMessage("MoveToSpawnPoint");

                return;
            }
                _animator.SetBool("Falling", true);
            if (_contact != null)
            {
                if (_contact.gameObject != null)
                {
                    if (_contact.gameObject.tag == "disposable")
                    {
                        _contact.gameObject.SendMessage("Break", jumpSpeed, SendMessageOptions.DontRequireReceiver);
                    }
                    _contact.gameObject.SendMessage("MoveUp", SendMessageOptions.DontRequireReceiver);
                }

            }
            if (Input.GetButtonDown("Jump") &&  _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "out")
            {

                if (_canDoubleJump && _gotDoubleJump)
                {
                    if(_jumping != null)
                        StopCoroutine(_jumping);
                    _jumping = StartCoroutine(AnimationByTime(transform));
                    _animator.SetBool("DoubleJump", true);

                    _canDoubleJump = false;
                }
            }

   
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }

            // workaround for standing on dropoff edge
            //if (_charController.isGrounded)
            //{
            //    if (Vector3.Dot(movement, _contact.normal) < 0)
            //    {
            //        movement = _contact.normal * moveSpeed;
            //    }
            //    else
            //    {
            //        movement += _contact.normal * moveSpeed;
            //    }
            //}
        }
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
            isntFake = true;

        }
        else if (hit.gameObject.tag == "fake")
        {
            isntFake = false;
            hit.gameObject.SendMessage("Break", jumpSpeed, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            isntFake = true;
        }
        hit.gameObject.SendMessage("MoveDown", SendMessageOptions.DontRequireReceiver);
    }

    private void ActivateDoubleJump()
    {
        _gotDoubleJump = true;
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
            if (progress > 0.85f)
            {
                _animator.SetBool("DoubleJump", false);
            }
            yield return null;
        }
        isJumping = false;
    }

}

using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float cameraSens;
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private KeyCode sitKeyCode;
    [SerializeField] private float cameraStandY;
    [SerializeField] private float cameraSitY;
    [SerializeField] private float crawlSlow;

    private bool _isCrawl;
    private bool _isGrounded;
    private Rigidbody _playerRigidbody;

    private void Start()
    {
        _playerRigidbody = gameObject.GetComponent<Rigidbody>();
        _isCrawl = false;
    }
    
    private void FixedUpdate()
    {
        Movement();
        Jump();
        Crawl();
        RotateGameObj();
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float v = cameraSens * Input.GetAxis("Mouse Y");
        cameraGameObject.transform.Rotate(v, 0, 0);
        
        float h = cameraSens * Input.GetAxis("Mouse X");
        Vector3 vector3 = new Vector3(0, h, 0);
        transform.Rotate(vector3);
    }

    private void RotateGameObj()
    {
        Transform gameObjectTransform = gameObject.transform;
        gameObjectTransform.Rotate(0, cameraSens * Input.GetAxis("Mouse X"), 0);
        _playerRigidbody.MoveRotation(gameObjectTransform.rotation);
    }

    private void Crawl()
    {
        //var position = cameraGameObject.transform.position;
        _isCrawl = Input.GetKey(sitKeyCode);
        //cameraGameObject.transform.position = _isCrawl ? new Vector3(position.x, cameraSitY, position.z) : new Vector3(position.x, cameraStandY, position.z);
        Debug.Log(_isCrawl ? "Сидит" : "Стоит");
    }

    private void Movement()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * -1;
        float moveZ = Input.GetAxis("Vertical") * speed * -1;

        if (_isCrawl)
        {
            moveX *= crawlSlow;
            moveZ *= crawlSlow;
        }

        Vector3 movement = new Vector3(moveX, 0.0f, moveZ);

        _playerRigidbody.AddForce(movement);
    }

    private void Jump()
    {
        if (!(Input.GetAxis("Jump") > 0)) return;
        
        if (_isGrounded)
        {
            _playerRigidbody.AddForce(Vector3.up * jumpForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IsGroundedCheck(collision, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        IsGroundedCheck(collision, false);
    }
    
    private void IsGroundedCheck(Collision collision, bool value)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = value;
        }
    }
}
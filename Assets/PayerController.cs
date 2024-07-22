using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PayerController : MonoBehaviour
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

    void Start()
    {
        _playerRigidbody = gameObject.GetComponent<Rigidbody>();
        _isCrawl = false;
    }
    
    void FixedUpdate()
    {
        Movement();
        Jump();
        RotateCamera();
        Crawl();
    }
    
    private void RotateCamera()
    {
        float h = cameraSens * Input.GetAxis("Mouse X");
        float v = cameraSens * Input.GetAxis("Mouse Y");
        transform.Rotate(0, h, 0);
        cameraGameObject.transform.Rotate(v, 0, 0);
    }

    private void Crawl()
    {
        var position = cameraGameObject.transform.position;
        _isCrawl = Input.GetKey(sitKeyCode);
        cameraGameObject.transform.position = _isCrawl ? new Vector3(position.x, cameraSitY, position.z) : new Vector3(position.x, cameraStandY, position.z);
        Debug.Log(_isCrawl ? "Сидит" : "Стоит");
    }

    private void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (_isCrawl)
        {
            moveX *= crawlSlow;
            moveZ *= crawlSlow;
        }

        Vector3 movement = new Vector3(moveX, 0.0f, moveZ);

        _playerRigidbody.AddForce(movement * speed);
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
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float MovementSpeed = 5.0f;
    float x, y;
    bool isGrounded = true;

    [Header("For GroundCheck & Jump")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRange = 0.3f;

    [SerializeField] float jumpHeight = 2.0f;
    [SerializeField] float jumpSpeed = 5.0f;


    public override void OnNetworkSpawn() // network equivalent of start
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            LobbyUI.Instance.ActivateLobbyUI(false);
            CameraController.Instance.InitializeCamera(transform);
        }
    }

    void Update()
    {
        if (!IsOwner || !IsSpawned) return; // makes sure rest of code is CLIENT AUTHORITY

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0, y);

        transform.position += Vector3.ClampMagnitude(movement, 1) * MovementSpeed * Time.deltaTime;


        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            StartCoroutine(jumping());
        }

    }

    bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckRange, groundLayer);
    }

    IEnumerator jumping()
    {
        Vector3 goalHeight = transform.position + Vector3.up * jumpHeight;
        while (transform.position.y < goalHeight.y)
        {
            transform.position += Vector3.up * jumpSpeed * Time.deltaTime;
            yield return null;
        }
        
        while (!isGrounded)
        {
            transform.position += Vector3.down * 9.82f * Time.deltaTime;
            isGrounded = GroundCheck();
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckRange, Color.green);
    }
}

using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharControllerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn() // network equivalent of start
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            LobbyUI.Instance.ActivateLobbyUI(false);
            CameraController.Instance.InitializeCamera(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner || !IsSpawned) return; // makes sure rest of code is CLIENT AUTHORITY

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep the character grounded
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move, 1); // Normalize diagonal movement
        
        move = transform.TransformDirection(move);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = (move * speed) + new Vector3(0, velocity.y, 0);
        
        controller.Move(finalMove * Time.deltaTime); 
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayTaunt_ServerRPC(SoundEffects.Taunt1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayTaunt_ServerRPC(SoundEffects.Taunt2);
        }
    }

    [ServerRpc]
    public void PlayTaunt_ServerRPC(SoundEffects anEffect)
    {
        BroadcastTaunt_ClientRPC(anEffect);
        Debug.Log("playing sound on server" + anEffect.ToString());
    }

    [ClientRpc]
    public void BroadcastTaunt_ClientRPC(SoundEffects anEffect)
    {
        SoundManager.Instance.PlaySoundEffect(anEffect);
        Debug.Log("playing sound on client" + anEffect.ToString());
    }
}

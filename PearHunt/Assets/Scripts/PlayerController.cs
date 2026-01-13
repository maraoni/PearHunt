using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float MovementSpeed = 5.0f;
    float x, y;

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
        Vector3 targetPosition = Vector3.ClampMagnitude(movement, 1) * MovementSpeed * Time.deltaTime;
        
        // Cause we wanna move based on direction we are facing (body)
        transform.position += transform.TransformDirection(targetPosition);
    }
}

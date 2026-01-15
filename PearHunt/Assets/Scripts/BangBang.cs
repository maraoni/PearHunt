using Unity.Netcode;
using UnityEngine;

public class BangBang : NetworkBehaviour
{
    [SerializeField] private float firerate = 0.5f;
    [SerializeField] private float damage = 10f;

    private float timer;

    [SerializeField] private LayerMask hiders;

    Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!IsOwner || !IsSpawned) return;
        animator = GetComponent<Animator>();
        transform.SetParent(CameraController.Instance.transform);
        transform.localPosition = new Vector3(0.5f, 0.3f, 0.7f);

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner || !IsSpawned) return;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && timer <= 0)
        {
            timer = firerate;

            Debug.Log("Clicked left mouse button");
            animator.SetTrigger("Shoot");

            PlayShooting_ServerRPC(SoundEffects.ShootingSound);
        }
    }

    private void OnBulletSpawn()
    {
        Debug.Log("Bullet shot");

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.gameObject.layer == hiders)
            {
                //take damage
                Debug.Log("Hit " + hitInfo.collider.gameObject.name + " for " + damage + " damage.");
                IDamageable damageable = hitInfo.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage((int)damage);
                }
            }
        }
        


    }

    private void OnReset()
    {
        Debug.Log("Can fire again");
    }

    [ServerRpc]
    public void PlayShooting_ServerRPC(SoundEffects anEffect)
    {
        BroadcastShooting_ClientRPC(anEffect);
        Debug.Log("playing sound on server" + anEffect.ToString());
    }

    [ClientRpc]
    public void BroadcastShooting_ClientRPC(SoundEffects anEffect)
    {
        SoundManager.Instance.PlaySoundEffect(anEffect);
        Debug.Log("playing sound on client" + anEffect.ToString());
    }
}

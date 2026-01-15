using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
class PropData
{
    public Mesh mesh;
    public bool inUse;
}

public class SwitchProp : NetworkBehaviour
{
    [SerializeField] PropData[] props;

    [SerializeField] KeyCode switchProp;

    BoxCollider propCollider;
    MeshFilter propRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        propRenderer = GetComponentInChildren<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner || !IsSpawned) return;

        if (Input.GetKeyDown(switchProp))
        {
            PropData newProp = GetNewProp();

            PropData currentProp = Array.Find(props, p => p.inUse == true);


            if (newProp != null)
            {
                if (currentProp != null) currentProp.inUse = false;
                
                newProp.inUse = true;
                //propRenderer.mesh = newProp.mesh.sharedMesh;

                for (int i = 0; i < props.Length; i++)
                {
                    if (props[i].inUse)
                    {
                        SendPropChange_ServerRPC(i, (int)OwnerClientId);
                    }
                }

                //propRenderer.transform.localScale = GetNewProp().transform.localScale;
            }

        }
    }

    private PropData GetNewProp()
    {
        PropData[] availableProps = Array.FindAll(props, p => !p.inUse);

        if (availableProps.Length == 0)
        {
            return null;
        }

        int value = Random.Range(0, availableProps.Length);


        return availableProps[value];
    }

    [ServerRpc]
    private void SendPropChange_ServerRPC(int propIndex, int playerIndex)
    {
        Broadcast_PropChange_ClientRPC(propIndex, playerIndex);
    }

    [ClientRpc]
    private void Broadcast_PropChange_ClientRPC(int propIndex, int playerIndex)
    {
        if ((int)OwnerClientId != playerIndex) return;

        propRenderer.mesh = props[propIndex].mesh;
        CalculateBounds();

        if (!IsOwner) return;
        CameraController.Instance.InitializeCamera(transform);
    }

    public void CalculateBounds()
    {
        Bounds bounds = propRenderer.mesh.bounds;

        Vector3 finalSize = bounds.size;
        Vector3 finalCenter = bounds.center;

        finalCenter.x *= propRenderer.transform.localScale.x;
        finalCenter.y *= propRenderer.transform.localScale.y;
        finalCenter.z *= propRenderer.transform.localScale.z;

        finalSize.x *= propRenderer.transform.localScale.x;
        finalSize.y *= propRenderer.transform.localScale.y;
        finalSize.z *= propRenderer.transform.localScale.z;

        GetComponent<BoxCollider>().size = finalSize * 0.9f;
        GetComponent<BoxCollider>().center = finalCenter;
    }
}


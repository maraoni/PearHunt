using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.LightTransport.PostProcessing;
using Random = UnityEngine.Random;

[Serializable]
class PropData
{
    public MeshFilter mesh;
    public bool inUse;
}

public class SwitchProp : MonoBehaviour
{
    [SerializeField] PropData[] props;

    [SerializeField] KeyCode switchProp;

    MeshFilter propRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        propRenderer = GetComponentInChildren<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(switchProp))
        {
            PropData newProp = GetNewProp();

            PropData currentProp = Array.Find(props, p => p.mesh.sharedMesh == propRenderer.sharedMesh);
            currentProp.inUse = false;

            if (newProp != null)
            {
                newProp.inUse = true;
                propRenderer.mesh = newProp.mesh.sharedMesh;
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

    public void RespawnProp()
    {
        transform.position = new Vector3(0, 0, 0);
    }
}

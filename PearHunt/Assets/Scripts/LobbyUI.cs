using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    #region Singleton
    public static LobbyUI Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField] Button m_StartHostButton;
    [SerializeField] Button m_StartClientButton;
    [SerializeField] TMP_InputField m_InputField;

    void Start()
    {
        m_StartHostButton.onClick.AddListener(StartHost);
        m_StartClientButton.onClick.AddListener(StartClient);
        m_InputField.onValueChanged.AddListener(ChangeIP);
        m_InputField.text = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address;
    }

    void StartClient()
    {
        
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client started");
        }
        else
        {
            Debug.LogError("Client failed to start");
        }
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        
    }

    void ChangeIP(string aInput)
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(aInput, 7777);
        // change the IP we want to connect to!
    }

    public void ActivateLobbyUI(bool should)
    {
        gameObject.SetActive(should);
    }
}

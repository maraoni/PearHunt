using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;




public class TeamsUi : MonoBehaviour
{
    public static TeamsUi Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private List<PlayerDate> players = new();
    //change the list of gameobject to a list of the script that holds the player info


  [SerializeField]  private TextMeshProUGUI Hunttext;
    [SerializeField] private TextMeshProUGUI PropText;


    private int maxHunters = 2;
    public void RandomTeam()
    {
        
        if (players.Count == 0) return;
       
        Debug.Log("randomising team");


        foreach (PlayerDate pl in players)
        {
            int randomnum = (int)Random.Range(0, 2);
            pl.Team.Value = randomnum; 
        }
       
        UpdateTeamUI();



    }
    public void UpdateTeamUI()
    {

        Hunttext.text = "";
        PropText.text = "";

        foreach (PlayerDate pl in players)
        {
            if (pl.Team.Value == 0)
            {
              
                    Hunttext.text +=  pl.name.ToString();
                
            }
            else if (pl.Team.Value == 1)
            {
                
                   PropText.text += pl.name.ToString();
                
            }
        }
    }

    public void Addplayers(PlayerDate pl)
    {
        players.Add(pl);
        UpdateTeamUI();
    }
    public void Start()
    {
        RandomTeam();
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("PlayTest", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }




}

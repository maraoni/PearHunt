using System.Collections.Generic;
using TMPro;
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


    private List<PlayerDate> players;
    //change the list of gameobject to a list of the script that holds the player info

    private PlayerDate Currentplayer;

  [SerializeField]  private TextMeshProUGUI Hunttext;
    [SerializeField] private TextMeshProUGUI PropText;

    private List<string> HunterTeam;
    private List<string> propTeam;
    private int maxHunters = 2;
    public void RandomTeam()
    {
        int randomnum = (int)Random.Range(0, 3);


        if (HunterTeam.Count >= maxHunters)
        {
            Currentplayer.Team.Value = 0;


        }
        else
        {
            if (randomnum == 0)
            {
                Debug.Log("You are on the Hunter Team");
                Currentplayer.Team.Value = 1;
            }
            else
            {
                Currentplayer.Team.Value = 0;
                Debug.Log("You are on the Prop Team");
            }
        }

       


    }
    public void UpdateTeamUI()
    {
        foreach (PlayerDate pl in players)
        {
            if (pl.Team.Value == 0)
            {
                if (!HunterTeam.Contains(pl.name))
                {
                    Hunttext.text +=  pl.name.ToString();
                }
            }
            else if (pl.Team.Value == 1)
            {
                if (!propTeam.Contains(pl.name))
                {
                    Hunttext.text += pl.name.ToString();
                }
            }
        }
       
        PropText.text = "Props: " + propTeam.Count.ToString();
    }


    public void Addplayers(PlayerDate pl)
    {
        players.Add(pl);
        Currentplayer = pl;
    }

    public void Broadcast_UptadeTeams_CliendRCP()
    {

    }


}

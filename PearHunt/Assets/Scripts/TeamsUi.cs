using System.Collections.Generic;
using UnityEngine;

public class TeamsUi : MonoBehaviour
{
    private List<string> HunterTeam;
    private List<string> propTeam;
    private int maxHunters = 2;
    public void RandomTeam()
    {
        int randomnum = (int)Random.Range(0, 3);


        if (HunterTeam.Count >= maxHunters)
        {



        }else
        {
            if (randomnum == 0)
            {
                Debug.Log("You are on the Hunter Team");

            }
            else
            {
                Debug.Log("You are on the Prop Team");
            }
        }

       


    }
}

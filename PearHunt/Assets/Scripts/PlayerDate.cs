using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerDate : NetworkBehaviour
{

    public NetworkVariable<FixedString64Bytes> PlayerName = new NetworkVariable<FixedString64Bytes> ("player");

    public NetworkVariable<int> PlayerHp = new NetworkVariable<int>(100);

    //  0 = prop,  1 = hunter
    public NetworkVariable<int> Team = new NetworkVariable<int>(0);

    void Start()
    {

        if (TeamsUi.Instance != null)
        {
            TeamsUi.Instance.Addplayers(this);
        }

        Team.OnValueChanged += UpdateUi;

        UpdateUi(default, Team.Value);

    }
    public void Respawn()
    {
        PlayerHp.Value = 100;
    }

    public void TakeDamage(int damage)
    {
        PlayerHp.Value -= damage;
        if (PlayerHp.Value < 0)
        {
            PlayerHp.Value = 0;
        }
    }

    public void ChangeTeam(int team)
    {
        Team.Value = team;
    }

    public void UpdateUi(int previousValue, int newValue)
    {
        if (TeamsUi.Instance != null)
        {
            TeamsUi.Instance.UpdateTeamUI();
        }
    }
    private void OnDestroy()
    {
        Team.OnValueChanged -= UpdateUi;
    }

}

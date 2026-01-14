using TMPro;
using Unity.Netcode;
using UnityEngine;

public class HunterHealth : NetworkBehaviour
{
    [SerializeField] private Transform playerUI;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text ammoText;

    [Header("Health Stats")]
    public int minHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] public int currrentHealth;

    [Header("Weapons and Ammo")]
    public int minAmmo;
    [SerializeField] public int maxAmmo;
    [SerializeField] public int currrentAmmo;
    [SerializeField] private Transform hunterWeapon;
    //Separate hunter and hider?




    void Start()
    {
        currrentHealth = maxHealth;
    }

    void Update()
    {
        healthText.text = currrentHealth.ToString();
        ammoText.text = currrentAmmo.ToString();
    }

    //Need functions for:
    //Shoot
    //Take damage
    //Ammo drop pickup?

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_UpgradeShop : MonoBehaviour
{
    public GameObject PlayerCanvas;

    public TMP_Text SpeedCost;
    public TMP_Text DamageCost;
    public TMP_Text JumpCost;
    public TMP_Text MagCost;
    public TMP_Text FirerateCost;

    public float SpeedUpgradeCost = 100.0f;
    public float JumpUpgradeCost = 1000.0f;

    public float DamageUpgradeCost = 1000.0f;
    public float FireRateUpgradeCost = 1000.0f;
    public float MagCountUpgradeCost = 1000.0f;

    private void Start()
    {
        PlayerCanvas = GameObject.FindGameObjectWithTag("PlayerUI");

        SpeedCost.text = "Cost: " + SpeedUpgradeCost.ToString("F0");
        JumpCost.text = "Cost: " + JumpUpgradeCost.ToString("F0");

        DamageCost.text = "Cost: " + DamageUpgradeCost.ToString("F0");
        FirerateCost.text = "Cost: " + FireRateUpgradeCost.ToString("F0");
        MagCost.text = "Cost: " + MagCountUpgradeCost.ToString("F0");

        
    }

    private void Awake()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    public Text energyText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        energyText.text = "PlayerEnergy:"+ GameObject.Find("Player").GetComponent<CombatData>().energy.ToString();
    }
}

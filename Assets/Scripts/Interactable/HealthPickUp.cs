using Assets.Scripts.Base_Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PickUp
{
    [Range(-1, 5)] public int HealingAmount = 1;
    
    

    private bool isPickedUp = false;

    
    
    

}

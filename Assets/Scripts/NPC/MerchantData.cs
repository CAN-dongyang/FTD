using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Merchant Data", menuName = "NPC/Merchant Data")]
public class MerchantData : ScriptableObject
{
    [Header("Merchant Info")]
    public string merchantName = "Merchant";
    
    [Header("Items for Sale")]
    public List<Item> sellItems = new List<Item>();
}

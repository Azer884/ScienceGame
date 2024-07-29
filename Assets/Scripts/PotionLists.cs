using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Lists", menuName = "ScriptableObjects/Lists", order = 15)]
public class PotionLists : ScriptableObject
{
    public List<string> stringLists = new List<string>();
}

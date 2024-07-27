using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LiquidPropertiesCollection", menuName = "ScriptableObjects/LiquidPropertiesCollection", order = 1)]
public class LiquidPropertiesCollection : ScriptableObject
{
    public List<LiquidProperties> liquidPropertiesList;
}

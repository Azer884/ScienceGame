using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 0)]
public class PlayerStats : ScriptableObject
{
    public float strength;
    public float HP;
    public float speed;
    public float attackSpeed;
    public float scale;
    public float jump;
    public float defense;
}

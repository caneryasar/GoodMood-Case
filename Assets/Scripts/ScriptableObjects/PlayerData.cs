using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject {

    public float freeMoveSpeed;
    public float lockOnMoveSpeed;
    public float rotationSpeed;

    public float baseDamage;
    public float finisherDamage;
}
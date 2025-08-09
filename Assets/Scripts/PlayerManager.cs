using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }
    
    public Tank.Tank player { get; set; }
    private void Awake() => instance = this;
}
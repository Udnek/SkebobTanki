using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    // TODO: PUT MAIN CAMERA
    private void Awake() => instance = this;
}
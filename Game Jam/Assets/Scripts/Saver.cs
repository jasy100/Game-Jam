using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    public bool GameOnGoing = false;
    public int Player1_score = 0;
    public int Player2_score = 0;
    public int Previous_Map = 0;

    public static Saver Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour {

    public bool occupied;
    public enum Player {Player1, Player2, None};
    public Player Owner;
    public Player Turn;
}

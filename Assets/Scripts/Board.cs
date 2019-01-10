using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Move
{
    //data to be recorded per move goes here
    public int Turn;
    public Logic.Player Player;
    public GameObject Tile;
}

public class Board : MonoBehaviour
{

    public List<GameObject> Tiles;
    public List<Move> Moves;

    public GameObject Manager;
    public bool Winner;
    public int Turns;
    public Logic.Player whowon;
    


    private void Start()
    {
        CoinFlip();
        UpdateTurn();
        Manager = GameObject.Find("Manager");
    }

    //cahnge tile to show player token
    //updates tile image and moves turn order
    public void ChangeTileImage()
    {
        GameObject M = Manager;
        Players P = M.GetComponent<Players>();

        GameObject G = EventSystem.current.currentSelectedGameObject.gameObject;
        Logic L = G.GetComponent<Logic>();

        Sprite Token = null;

        if (!Winner)
        {
            if (L.Turn == Logic.Player.Player1)
            {
                Token = P.Player1_Token;
            }
            else
            {
                Token = P.Player2_Token;

            }

            if (!L.occupied)
            {
                G.GetComponent<Image>().sprite = Token;
                L.occupied = true;
                L.Owner = L.Turn;
                PlaySound();
                RecordMoves();
                UpdateTurn();    
            }
        }

    }

    //flip a coin to chose who goes first
    private void CoinFlip()
    {
        Logic.Player P = Logic.Player.Player1;

        int r = Random.Range(0, 10);
        if (r >= 5)
        {
            P = Logic.Player.Player1;
        }
        else
        {
            P = Logic.Player.Player2;
        }

        for (int i = 0; i < Tiles.Count; i++)
        {
            Logic L = Tiles[i].GetComponent<Logic>();
            L.Turn = P;
        }
    }

    //change players for next turn
    void UpdateTurn()
    {
        if (!Winner)
        {
            Ui UI = Manager.GetComponent<Ui>();

            for (int i = 0; i < Tiles.Count; i++)
            {
                Logic L = Tiles[i].GetComponent<Logic>();

                if (L.Turn == Logic.Player.Player1)
                {
                    L.Turn = Logic.Player.Player2;
                }
                else
                {
                    L.Turn = Logic.Player.Player1;
                }
            }

            CheckForWin();
            UI.ShowTurn();
            Turns += 1;
        }
    }

    //check if there is a winner
    public void CheckForWin()
    {
        //rows
        Logic.Player[] TopRow = { Tiles[0].GetComponent<Logic>().Owner, Tiles[1].GetComponent<Logic>().Owner, Tiles[2].GetComponent<Logic>().Owner };
        Logic.Player[] MidRow = { Tiles[3].GetComponent<Logic>().Owner, Tiles[4].GetComponent<Logic>().Owner, Tiles[5].GetComponent<Logic>().Owner };
        Logic.Player[] BotRow = { Tiles[6].GetComponent<Logic>().Owner, Tiles[7].GetComponent<Logic>().Owner, Tiles[8].GetComponent<Logic>().Owner };

        //columns
        Logic.Player[] LeftColumn = { Tiles[0].GetComponent<Logic>().Owner, Tiles[3].GetComponent<Logic>().Owner, Tiles[6].GetComponent<Logic>().Owner };
        Logic.Player[] MidColumn = { Tiles[1].GetComponent<Logic>().Owner, Tiles[4].GetComponent<Logic>().Owner, Tiles[7].GetComponent<Logic>().Owner };
        Logic.Player[] RightColumn = { Tiles[2].GetComponent<Logic>().Owner, Tiles[5].GetComponent<Logic>().Owner, Tiles[8].GetComponent<Logic>().Owner };

        //diagonals
        Logic.Player[] RightDiag = { Tiles[2].GetComponent<Logic>().Owner, Tiles[4].GetComponent<Logic>().Owner, Tiles[6].GetComponent<Logic>().Owner };
        Logic.Player[] LeftDiag = { Tiles[0].GetComponent<Logic>().Owner, Tiles[4].GetComponent<Logic>().Owner, Tiles[8].GetComponent<Logic>().Owner };

        //all combos
        List<Logic.Player[]> Combinations = new List<Logic.Player[]> { TopRow, MidRow, BotRow, LeftColumn, MidColumn, RightColumn, RightDiag, LeftDiag };

        for (int i = 0; i < Combinations.Count; i++)
        {
            if (Combinations[i][0] == Combinations[i][1] && Combinations[i][0] == Combinations[i][2])
            {
                if (Combinations[i][0] != Logic.Player.None)
                {
                    Winner = true;
                    whowon = Combinations[i][0];
                }
            }
        }

    }

    //start a new game
    public void NewGame()
    {
        PlaySound();
        Application.LoadLevel(0);
    }

    //exit application
    public void QuitGame()
    {
        PlaySound();
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    //play sound
    public void PlaySound()
    {
        AudioSource A = Manager.GetComponent<AudioSource>();
        A.PlayOneShot(A.clip);
           
    }

    //record match history
    public void RecordMoves()
    {
        Move M = new Move();
        M.Turn = Turns;
        M.Tile = EventSystem.current.currentSelectedGameObject.gameObject;
        M.Player = M.Tile.GetComponent<Logic>().Owner;
        Moves.Add(M);
    }
}

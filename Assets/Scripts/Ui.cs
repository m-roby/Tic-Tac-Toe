using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ui : MonoBehaviour {

    public GameObject HeaderText;
    public GameObject Popup;

    public void ShowTurn()
    {
        
        GameObject Manager = GameObject.Find("Manager");
        Board B = Manager.GetComponent<Board>();
        TextMeshProUGUI U = HeaderText.GetComponent<TextMeshProUGUI>();

        if(!B.Winner)
        {
            //show the turn
            U.SetText(B.Tiles[0].GetComponent<Logic>().Turn.ToString() + "'S TURN");

            //show a draw condition
            if(B.Turns == 9)
            {
                U.SetText("DRAW!");
                Popup.SetActive(true);
            }
        }
        else
        {
            //show the win screen
            U.SetText(B.whowon.ToString() + " WINS!");
            Popup.SetActive(true);
        }
            
    }


}

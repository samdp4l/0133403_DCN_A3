using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MyNetworkManager : NetworkManager
{
    public TextMeshProUGUI scoreboard;
    public List<string> playerList = new List<string>();
    public List<int> playerScore = new List<int>();

    public GameObject scorePrefab;
    public List<Transform> spawners = new List<Transform>();

    public override void Start()
    {
        base.Start();

        InvokeRepeating("Spawn", 5f, 5f);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        Debug.Log ("I connected to a server");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        //Debug.Log ("Player added to Server");
        //Debug.Log ($"There are now {numPlayers} players");
        
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        player.setDisplayName ($"Player {numPlayers}");

        Color displayColor = new Color (Random.Range (0f,1f), Random.Range(0f,1f), 
        Random.Range(0f,1f));

        player.setDisplayColor (displayColor);

        playerScore.Add(player.setDisplayScore());
        playerList.Add("Player " + numPlayers);

        string scoreboardText = "Active Players" + "\n";

        for (int i = 0; i < numPlayers; i++)
        {
            scoreboardText += playerList[i] + "\n";
        }

        scoreboard.text = scoreboardText;

        player.setIndex(numPlayers - 1);
    }

    public void Spawn()
    {
        int currentInt = 0;

        if (currentInt >= 2)
        {
            currentInt = 0;
        }
        else
        {
            currentInt++;
        }

        Instantiate(scorePrefab, spawners[currentInt].position, Quaternion.identity);
    }
}

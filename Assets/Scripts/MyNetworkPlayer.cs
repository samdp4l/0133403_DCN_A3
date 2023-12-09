using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Mirror;
using Telepathy;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    public TextMeshPro scoreText;

    [SerializeField] private TMP_Text nameText = null;    
    [SerializeField] private Renderer colorRenderer = null;
    [SerializeField] private int playerScore;
    [SerializeField] private int playerIndex;

    [SyncVar (hook = nameof(HandleDisplayNameUpdate))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar (hook = nameof(HandleDisplayColourUpdate))]
    [SerializeField]
    private Color displayColor = Color.black;

    [SyncVar(hook = nameof(HandleDisplayScoreUpdate))]
    [SerializeField]
    private int displayScore = 0;

    #region server
    [Server]
    public void setDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void setDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Server]
    public int setDisplayScore()
    {
        return displayScore;
    }

    [Server]
    public void setIndex(int newIndex)
    {
        playerIndex = newIndex;
    }

    [Command]
    private void CmdSetDisplayName (string newDisplayName)
    {
        //server authority to limit displayName into 2-20 letter length
        if (newDisplayName.Length < 2 || newDisplayName.Length > 20)
        {
            return;
        }
        RpcDisplayNewName(newDisplayName);
        setDisplayName (newDisplayName);
    }

    #endregion

    #region client
    private void HandleDisplayColourUpdate (Color oldColor, Color newColor)
    {
        colorRenderer.material.color = newColor;
    }

    private void HandleDisplayNameUpdate (string oldName, string newName)
    {
        nameText.text = newName;
    }

    private void HandleDisplayScoreUpdate(int oldScore, int newScore)
    {
        playerScore = newScore;
    }

    [ContextMenu ("Set This Name")]
    private void SetThisName()
    {
        CmdSetDisplayName("This is a new name");
    }

    [ClientRpc]
    private void RpcDisplayNewName (string newDisplayName)
    {
        Debug.Log (newDisplayName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Score"))
        {
            playerScore++;

            Destroy(collision.gameObject);
        }
    }
    #endregion

    private void Update()
    {
        scoreText.text = playerScore.ToString();
    }
}

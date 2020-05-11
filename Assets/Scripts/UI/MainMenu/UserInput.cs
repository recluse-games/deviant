using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private string server;
    private string playerId;

    public string GetServer()
    {
        return this.server;
    }

    public void SetServer(string server)
    {
        this.server = server;
    }

    public string GetPlayerId()
    {
        return this.playerId;
    }

    public void SetPlayer(string playerId)
    {
        this.playerId = playerId;
    }
}

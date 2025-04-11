using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public PlayerManager player1;
    public PlayerManager player2;

    public int rounds = 3;
    public int round = 0;

    public List<RoundResult> results;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        results = new List<RoundResult>();
        for (int i = 0; i < rounds; i++)
        {
            results.Add(RoundResult.NotDone);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewRound(RoundResult res)
    {
        results[round] = res;
        round++;
        player1.NewRound();
        player2.NewRound();
    }
}

public enum RoundResult
{
    NotDone,
    Player1,
    Player2,
    Draw
}
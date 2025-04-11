using UnityEngine;
using UnityEngine.UI;

public class HUDControler : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private bool isPlayer2;
    private PlayerManager playerManager;

    [SerializeField] private Image[] health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isPlayer2)
        {
            playerManager = battleManager.player2;
        }
        else
        {
            playerManager = battleManager.player1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < health.Length; i++)
        {
            if (i < playerManager.HP)
            {
                health[i].enabled = true;
            }
            else
            {
                health[i].enabled = false;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public bool isPlayer1;

    public int HP;
    public int WeaponHP;
    public float speed;

    public int leaning;

    public InputAction move;
    public InputAction lean;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputActionMap playerMap = isPlayer1 ? InputSystem.actions.FindActionMap("Player1") : InputSystem.actions.FindActionMap("Player2");
        move = playerMap.FindAction("Move");
        lean = playerMap.FindAction("lean");
    }

    // Update is called once per frame
    void Update()
    {
        if(move.ReadValue<float>() > 0)
        {
            transform.position += Vector3.right*speed*Time.deltaTime;
        }
        else if (move.ReadValue<float>() < 0)
        {
            transform.position -= Vector3.right*speed * Time.deltaTime;
        }

        leaning = (int)lean.ReadValue<float>();


    }
}

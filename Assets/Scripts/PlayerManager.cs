using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public bool isPlayer1;

    public int HP;
    public int WeaponHP;

    public int leaning;

    public InputAction move;
    public InputAction lean;
    public InputAction lunge;
    public InputAction thrust;

    private Animator animator;

    private int walkIndex;
    private int lungeIndex;
    private int thrustIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputActionMap playerMap = isPlayer1 ? InputSystem.actions.FindActionMap("Player1") : InputSystem.actions.FindActionMap("Player2");
        move = playerMap.FindAction("Move");
        lean = playerMap.FindAction("Lean");
        lunge = playerMap.FindAction("Lunge");
        thrust = playerMap.FindAction("Attack1");
        animator = GetComponent<Animator>();
        walkIndex = Animator.StringToHash("walk");
        lungeIndex = Animator.StringToHash("lunge");
        thrustIndex = Animator.StringToHash("thrust");

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1)
        {
            if(move.ReadValue<float>() > 0)
            {
                animator.SetInteger(walkIndex, 1);
            }
            else if (move.ReadValue<float>() < 0)
            {
                animator.SetInteger(walkIndex, -1);
            }
            else
            {
                animator.SetInteger(walkIndex, 0);
            }
        }
        else
        {
            if (move.ReadValue<float>() > 0)
            {
                animator.SetInteger(walkIndex, -1);
            }
            else if (move.ReadValue<float>() < 0)
            {
                animator.SetInteger(walkIndex, 1);
            }
            else
            {
                animator.SetInteger(walkIndex, 0);
            }
        }

        leaning = (int)lean.ReadValue<float>();
        

        animator.SetBool(lungeIndex,lunge.IsPressed());

        if (thrust.WasPressedThisFrame())
        {
            animator.SetTrigger(thrustIndex);
        }

    }

    public void MoveForAnimation(int pixels)
    {
        transform.position += Vector3.right * pixels * 0.03125f * (isPlayer1 ? 1 : -1);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public bool isPlayer1;

    public int HP;
    public int WeaponHP;

    public int leaning;

    public InputAction moveInput;
    public InputAction leanInput;
    public InputAction lungeInput;
    public InputAction thrustInput;
    public InputAction verticalInput;
    public InputAction blockInput;


    private Animator animator;

    private int walkIndex;
    private int lungeIndex;
    private int thrustIndex;
    private int verticalIndex;
    private int blockIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputActionMap playerMap = isPlayer1 ? InputSystem.actions.FindActionMap("Player1") : InputSystem.actions.FindActionMap("Player2");
        moveInput = playerMap.FindAction("Move");
        leanInput = playerMap.FindAction("Lean");
        lungeInput = playerMap.FindAction("Lunge");
        thrustInput = playerMap.FindAction("Attack1");
        verticalInput = playerMap.FindAction("Vertical");
        blockInput = playerMap.FindAction("Parry");

        animator = GetComponent<Animator>();

        walkIndex = Animator.StringToHash("walk");
        lungeIndex = Animator.StringToHash("lunge");
        thrustIndex = Animator.StringToHash("thrust");
        verticalIndex = Animator.StringToHash("vertical");
        blockIndex = Animator.StringToHash("block");

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1)
        {
            if(moveInput.ReadValue<float>() > 0)
            {
                animator.SetInteger(walkIndex, 1);
            }
            else if (moveInput.ReadValue<float>() < 0)
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
            if (moveInput.ReadValue<float>() > 0)
            {
                animator.SetInteger(walkIndex, -1);
            }
            else if (moveInput.ReadValue<float>() < 0)
            {
                animator.SetInteger(walkIndex, 1);
            }
            else
            {
                animator.SetInteger(walkIndex, 0);
            }
        }

        leaning = (int)leanInput.ReadValue<float>();
        

        animator.SetBool(lungeIndex,lungeInput.IsPressed());

        animator.SetBool(blockIndex,blockInput.IsPressed());

        animator.SetInteger(verticalIndex,(int)verticalInput.ReadValue<float>());

        if (thrustInput.WasPressedThisFrame())
        {
            animator.SetTrigger(thrustIndex);
        }

    }

    public void MoveForAnimation(int pixels)
    {
        transform.position += Vector3.right * pixels * 0.03125f * (isPlayer1 ? 1 : -1);
    }
}
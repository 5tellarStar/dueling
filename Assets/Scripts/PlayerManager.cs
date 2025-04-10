using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public bool isPlayer1;
    [SerializeField] private PlayerManager otherPlayer;

    public int HP;

    public int blockingState;

    public int highHitWidth;
    public int lowHitWidth;

    public InputAction moveInput;
    public InputAction leanInput;
    public InputAction lungeInput;
    public InputAction thrustInput;
    public InputAction verticalInput;


    private Animator animator;

    private int walkIndex;
    private int lungeIndex;
    private int thrustIndex;
    private int verticalIndex;
    private int horizontalIndex;
    private int hitIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputActionMap playerMap = isPlayer1 ? InputSystem.actions.FindActionMap("Player1") : InputSystem.actions.FindActionMap("Player2");
        moveInput = playerMap.FindAction("Move");
        leanInput = playerMap.FindAction("Lean");
        lungeInput = playerMap.FindAction("Lunge");
        thrustInput = playerMap.FindAction("Attack1");
        verticalInput = playerMap.FindAction("Vertical");

        animator = GetComponent<Animator>();

        walkIndex = Animator.StringToHash("walk");
        lungeIndex = Animator.StringToHash("lunge");
        thrustIndex = Animator.StringToHash("thrust");
        verticalIndex = Animator.StringToHash("vertical");
        horizontalIndex = Animator.StringToHash("horizontal");
        hitIndex = Animator.StringToHash("hit");

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1)
        {
            if (moveInput.ReadValue<float>() > 0)
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

        animator.SetInteger(horizontalIndex, (int)leanInput.ReadValue<float>());

        animator.SetBool(lungeIndex, lungeInput.IsPressed());

        animator.SetInteger(verticalIndex, (int)verticalInput.ReadValue<float>());

        if (thrustInput.WasPressedThisFrame())
        {
            animator.SetTrigger(thrustIndex);
        }

    }

    public void MoveForAnimation(int pixels)
    {
        transform.position += Vector3.right * pixels * 0.03125f * (isPlayer1 ? 1 : -1);
    }

    public void SetBlockState(int state)
    {
        blockingState = state;
    }

    public void SetHighHitWidth(int pixels)
    {
        highHitWidth = pixels;
    }

    public void SetLowHitWidth(int pixels)
    {
        lowHitWidth = pixels;
    }

    public void Attack(string StateAndRange)
    {
        int state = int.Parse(StateAndRange.Remove(1));

        int range = int.Parse(StateAndRange.Remove(0,2));

        int distInPixels = (int)(Mathf.Abs(transform.position.x - otherPlayer.transform.position.x) / 0.03125f);

        if((distInPixels < (range + otherPlayer.highHitWidth) && state == 2) || (distInPixels < (range + otherPlayer.lowHitWidth) && state == 0))
        {
            if(state != otherPlayer.blockingState)
            {
                otherPlayer.HP -= 1;
                otherPlayer.Hit();
                animator.SetTrigger(hitIndex);
            }
        }
    }

    public void Hit()
    {
        animator.SetTrigger(hitIndex);
    }
}
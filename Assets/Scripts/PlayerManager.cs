using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public bool isPlayer1;
    [SerializeField] private PlayerManager otherPlayer;

    public int HP;
    public int WeaponHP;

    public Vector2Int blockingState;

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
        switch (state)
        {
            case 0:
                blockingState = new Vector2Int(-1, 1);
                break;
            case 1:
                blockingState = new Vector2Int(0, 1);
                break;
            case 2:
                blockingState = new Vector2Int(1, 1);
                break;
            case 3:
                blockingState = new Vector2Int(-1, 0);
                break;
            case 4:
                blockingState = new Vector2Int(0, 0);
                break;
            case 5:
                blockingState = new Vector2Int(1, 0);
                break;
            case 6:
                blockingState = new Vector2Int(-1, -1);
                break;
            case 7:
                blockingState = new Vector2Int(0, -1);
                break;
            case 8:
                blockingState = new Vector2Int(1, -1);
                break;
        }
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
        int stateInt = int.Parse(StateAndRange.Remove(1));
        Vector2Int state = Vector2Int.zero;
        switch (stateInt)
        {
            case 0:
                state = new Vector2Int(-1, 1);
                break;
            case 1:
                state = new Vector2Int(0, 1);
                break;
            case 2:
                state = new Vector2Int(1, 1);
                break;
            case 3:
                state = new Vector2Int(-1, 0);
                break;
            case 4:
                state = new Vector2Int(0, 0);
                break;
            case 5:
                state = new Vector2Int(1, 0);
                break;
            case 6:
                state = new Vector2Int(-1, -1);
                break;
            case 7:
                state = new Vector2Int(0, -1);
                break;
            case 8:
                state = new Vector2Int(1, -1);
                break;
        }
        int range = int.Parse(StateAndRange.Remove(0,2));

        int distInPixels = (int)(Mathf.Abs(transform.position.x - otherPlayer.transform.position.x) / 0.03125f);

        if((distInPixels < (range + otherPlayer.highHitWidth) && (state.y == 1|| state.y == 0)) || (distInPixels < (range + otherPlayer.lowHitWidth) && state.y == -1))
        {
            if(state != otherPlayer.blockingState)
            {
                otherPlayer.HP -= 1;
            }
        }
    }
}
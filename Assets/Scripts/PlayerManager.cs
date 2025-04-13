using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public bool isPlayer1;
    [SerializeField] private PlayerManager otherPlayer;
    [SerializeField] private BattleManager battleManager;

    public int HP;

    public int blockingState;

    public int attackState = 1;
    public int range = 0;


    public int swordHitWidth;
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
        int distInPixels = (int)(Mathf.Abs(transform.position.x - otherPlayer.transform.position.x) / 0.03125f);

        if (isPlayer1)
        {
            if (moveInput.ReadValue<float>() > 0 && distInPixels > 71)
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
            else if (moveInput.ReadValue<float>() < 0 && distInPixels > 71)
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

        Debug.Log(distInPixels);

        if ((distInPixels < (range + otherPlayer.highHitWidth + otherPlayer.swordHitWidth) && attackState == 2) || (distInPixels < (range + otherPlayer.lowHitWidth + otherPlayer.swordHitWidth) && attackState == 0))
        {
            if (attackState != otherPlayer.blockingState)
            {
                if ((distInPixels < (range + otherPlayer.highHitWidth) && attackState == 2) || (distInPixels < (range + otherPlayer.lowHitWidth) && attackState == 0))
                {
                    otherPlayer.HP -= 1;
                    if (otherPlayer.HP <= 0)
                    {
                        if (isPlayer1)
                        {
                            battleManager.NewRound(RoundResult.Player1);
                        }
                        else
                        {
                            battleManager.NewRound(RoundResult.Player2);
                        }
                    }
                    else
                    {
                        otherPlayer.Hit();
                        animator.SetTrigger(hitIndex);
                    }
                }
            }
            else
            {
                otherPlayer.transform.position += Vector3.right * (isPlayer1 ? 1 : -1) * 5 * 0.03125f;
            }
            attackState = 1;
            range = 0;
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
        attackState = int.Parse(StateAndRange.Remove(1));

        range = int.Parse(StateAndRange.Remove(0,2));
    }

    public void AttackStop()
    {
        attackState = 1;
        range = 0;
    }

    public void Hit()
    {
        animator.SetTrigger(hitIndex);
    }

    public void NewRound()
    {
        HP = 3;
        transform.position = new Vector3(-3.25f * transform.localScale.x,0,0);
        animator.Rebind();
        animator.Update(0f);
    }
}
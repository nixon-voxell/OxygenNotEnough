using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController m_CharController;
    [SerializeField] private float m_WalkSpeed = 5.0f;
    [SerializeField] private float m_CrouchSpeed = 2.0f;
    [SerializeField] private float m_rotationSpeed = 720f;
    [SerializeField] private Animator m_Animator;

    private bool m_IsMoving;
    private bool m_IsCrouching;
    private bool m_IsUsingHelium;
    public bool IsMoving => this.m_IsMoving;
    public bool IsCrouching => this.m_IsCrouching;
    public bool IsUsingHelium => this.m_IsUsingHelium;

    public void Move(Vector3 moveDirection)
    {
        float magnitude = this.m_WalkSpeed;
        if (this.m_IsCrouching)
        {
            magnitude = this.m_CrouchSpeed;
        }
        magnitude *= Time.deltaTime;

        this.m_CharController.Move(moveDirection * magnitude);
    }

    private void Start()
    {
        this.m_IsCrouching = false;
        this.m_IsMoving = false;
        this.m_IsUsingHelium = false;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.InProgress) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
        moveDirection.Normalize();

        this.m_IsMoving = moveDirection.sqrMagnitude > 0.0f;
        this.m_IsCrouching = !Input.GetButton("Crouch");
        this.m_IsUsingHelium = Input.GetButtonDown("Jump");
        
        if (this.m_IsMoving)
        {
            Quaternion torotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, torotation, m_rotationSpeed * Time.deltaTime);
            this.Move(moveDirection);

            if (this.m_IsCrouching)
            {
                this.m_Animator.Play("CrouchWalk");
            } else
            {
                this.m_Animator.Play("Walk");
            }
        } else
        {
            if (this.m_IsCrouching)
            {
                this.m_Animator.Play("CrouchIdle");
            } else
            {
                this.m_Animator.Play("Idle");
            }
        }
    }

}

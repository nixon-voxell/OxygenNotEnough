using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private CharacterController m_CharController;
    [SerializeField] private float m_WalkSpeed = 5.0f;
    [SerializeField] private float m_CrouchSpeed = 2.0f;

    private bool m_IsMoving;
    private bool m_IsCrouching;

    public bool IsMoving => this.m_IsMoving;
    public bool IsCrouching => this.m_IsCrouching;

    void Start()
    {
        this.m_IsCrouching = false;
        this.m_IsMoving = false;
    }

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

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
        moveDirection.Normalize();

        this.m_IsMoving = moveDirection.sqrMagnitude > 0.0f;
        this.m_IsCrouching = Input.GetButton("Crouch");

        this.Move(moveDirection);
    }

}

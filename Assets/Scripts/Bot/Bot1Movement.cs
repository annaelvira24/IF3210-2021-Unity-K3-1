using UnityEngine;

public class Bot1Movement : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_Speed = 12f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.


    [HideInInspector] public GameObject m_TargetTank;
    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        // The axes names are based on player number.

    }


    private void Update()
    {
        // Store the value of both input axes.
        Move();

    }


    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        Turn();
    }


    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        //Vector3 movement = transform.towards * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
 //       m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        if(Mathf.Pow(Mathf.Pow(transform.position.z - m_TargetTank.transform.position.z, 2f) + Mathf.Pow(transform.position.x - m_TargetTank.transform.position.x, 2f), 0.5f) > 8f)
        {
            m_Speed = 0.3f;
            m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, m_TargetTank.transform.TransformPoint(new Vector3(0f, 0f, -5)), m_Speed));
        }
        else
        {
            m_Speed = 0;
        }

    }


    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        //float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        //Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        //m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        m_Rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, m_TargetTank.transform.rotation, m_TurnSpeed));
    }
}
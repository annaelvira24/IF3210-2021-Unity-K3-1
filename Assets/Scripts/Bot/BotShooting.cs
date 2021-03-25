using UnityEngine;
using UnityEngine.UI;

public class BotShooting : MonoBehaviour
{
    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.


    private int m_ShootTime;

    private void OnEnable()
    {
        // When the tank is turned on, reset the launch force and the UI
        m_ShootTime = 0;
    }

    private void Start()
    {
        m_ShootTime = 0;
    }


    private void Update()
    {
        // The slider should have a default value of the minimum launch force.
        if (m_ShootTime% 100 == 0)
        {
            // ... launch the shell.
            Fire();
        }
        m_ShootTime++;
    }


    private void Fire()
    {
        // Set the fired flag so only Fire is only called once.

        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = 20.0f * m_FireTransform.forward; ;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
    }
}

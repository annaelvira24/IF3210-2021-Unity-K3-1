using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;            // The number of rounds a single player has to win to win the game.
    public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
    public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
    public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
    public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
    public TankManager[] m_Tanks;               // A collection of managers for enabling and disabling different aspects of the tanks.
    public GameObject m_Bot1Prefab;
    public Bot1Manager[] m_Bots1;
    public GameObject m_Bot2Prefab;
    public Bot2Manager[] m_Bots2;
    public Canvas m_StartMenu;
    public Canvas m_SettingsMenu;
    public Button m_PlayButton;
    public Button m_SettingsButton;
    public Button m_OkSettingsButton;
    public Button m_QuitButton;
    public Slider m_SoundSlider;
    public AudioMixer masterMixer;
    public Canvas m_GamePlayMenu;
    public Button m_Bot1;
    public Button m_Bot2;
    public Dropdown m_MapDropDown;
    public GameObject m_Map1Prefab;
    public GameObject m_Map2Prefab;

    private int m_PlayerNow;
    private int m_Bot1Count;
    private int m_Bot2Count;
    private int m_MapNumber;
    private GameObject m_MapInstance;
    private int m_RoundNumber;                  // Which round the game is currently on.
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
    private TankManager m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
    private TankManager m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.



    private void Start()
    {
        m_SettingsMenu.enabled = false;
        m_GamePlayMenu.enabled = false;
        m_PlayButton.onClick.AddListener(PlayGame);
        m_SettingsButton.onClick.AddListener(SettingsMenu);
        m_OkSettingsButton.onClick.AddListener(QuitSettings);
        m_SoundSlider.onValueChanged.AddListener(delegate { SoundSettings(); });
        m_MapDropDown.onValueChanged.AddListener(delegate { MapSettings(); });
        m_Bot1.onClick.AddListener(SpawnBot1);
        m_Bot2.onClick.AddListener(SpawnBot2);
        m_MapNumber = 1;
        m_Bot1Count = 0;
        m_Bot2Count = 0;
        m_MapInstance = Instantiate(m_Map1Prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    }

    private void PlayGame()
    {
        m_StartMenu.enabled = false;
        m_PlayButton.enabled = false;
        m_GamePlayMenu.enabled = true;

        // Create the delays so they only have to be made once.
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        m_PlayerNow = 2;

        SpawnAllTanks();
        SpawnMap();
        SetCameraTargets();

        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine(GameLoop());
    }

    private void SettingsMenu()
    {
        m_SettingsMenu.enabled = true;
    }

    private void SoundSettings()
    {
        masterMixer.SetFloat("MusicVolume", m_SoundSlider.value);
        masterMixer.SetFloat("SFXVolume", 0 + m_SoundSlider.value);
        masterMixer.SetFloat("DrivingVolume", -25 + m_SoundSlider.value);
    }

    private void MapSettings()
    {
        if(m_MapDropDown.value == 0)
        {
            m_MapNumber = 1;
        }
        else if (m_MapDropDown.value == 1)
        {
            m_MapNumber = 2;
        }
    }

    private void QuitSettings()
    {
        m_SettingsMenu.enabled = false;
    }

    private void SpawnMap()
    {

        if(m_MapNumber == 2)
        {
            m_MapInstance.SetActive(false);
            m_MapInstance = Instantiate(m_Map2Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    private void SpawnAllTanks()
    {
        // For all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... create them, set their player number and references needed for control.
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }

    private void SpawnBot1()
    {
        // ... create them, set their player number and references needed for control.
            m_Bots1[m_Bot1Count].m_Instance =
                Instantiate(m_Bot1Prefab, m_Tanks[(m_PlayerNow)%2].m_Instance.transform.TransformPoint(new Vector3(0f,0f,-5)), m_Tanks[(m_PlayerNow) % 2].m_Instance.transform.rotation) as GameObject;
            m_Bots1[m_Bot1Count].m_PlayerNumber = m_PlayerNow;
            m_Bots1[m_Bot1Count].m_TargetTank = m_Tanks[(m_PlayerNow) % 2].m_Instance;

            m_Bots1[m_Bot1Count].Setup();

            m_Bot1Count++;
    }

    private void SpawnBot2()
    {
        int count = 0;
        Vector3 botSpawn = new Vector3(0, 0, 0);
        for (int i = 0; i<m_Bot2Count; i++)
        {
            if(m_Bots2[i].m_PlayerNumber == m_PlayerNow)
            {
                count++;
            }
        }

        if (count % 2 == 0)
        {
            botSpawn = new Vector3(3, 0, 3);
            m_Bots2[m_Bot2Count].m_BotNumber = 0;
        }
        else
        {
            botSpawn = new Vector3(-3, 0, 3);
            m_Bots2[m_Bot2Count].m_BotNumber = 1;
        }
        m_Bots2[m_Bot2Count].m_Instance =
            Instantiate(m_Bot2Prefab, m_Tanks[(m_PlayerNow - 1)].m_Instance.transform.transform.TransformPoint(botSpawn), m_Tanks[(m_PlayerNow - 1)].m_Instance.transform.rotation) as GameObject;
        m_Bots2[m_Bot2Count].m_PlayerNumber = m_PlayerNow;
        m_Bots2[m_Bot2Count].m_PlayerTank = m_Tanks[(m_PlayerNow-1)].m_Instance;

        m_Bots2[m_Bot2Count].Setup();

        m_Bot2Count++;
    }


    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[m_Tanks.Length];

        // For each of these transforms...
        for (int i = 0; i < targets.Length; i++)
        {
            // ... set it to the appropriate tank transform.
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        // These are the targets the camera should follow.
        m_CameraControl.m_Targets = targets;
    }


    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (m_GameWinner != null)
        {
            // If there is a game winner, restart the level.
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            // If there isn't a winner yet, restart this coroutine so the loop continues.
            // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        // As soon as the round starts reset the tanks and make sure they can't move.
        ResetAllTanks();
        ResetAllBots();
        DisableTankControl();

        // Snap the camera's zoom and position to something appropriate for the reset tanks.
        m_CameraControl.SetStartPositionAndSize();

        // Increment the round number and display text showing the players what round it is.
        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        // As soon as the round begins playing let the players control the tanks.
        EnableTankControl();

        // Clear the text from the screen.
        m_MessageText.text = string.Empty;

        // While there is not one tank left...
        while (!OneTankLeft())
        {
            // ... return on the next frame.
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        // Stop tanks from moving.
        DisableTankControl();

        // Clear the winner from the previous round.
        m_RoundWinner = null;

        // See if there is a winner now the round is over.
        m_RoundWinner = GetRoundWinner();

        // If there is a winner, increment their score.
        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        // Now the winner's score has been incremented, see if someone has one the game.
        m_GameWinner = GetGameWinner();

        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessage();
        m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_EndWait;
    }


    // This is used to check if there is one or fewer tanks remaining and thus the round should end.
    private bool OneTankLeft()
    {
        // Start the count of tanks left at zero.
        int numTanksLeft = 0;

        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if they are active, increment the counter.
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        // If there are one or fewer tanks remaining return true, otherwise return false.
        return numTanksLeft <= 1;
    }


    // This function is to find out if there is a winner of the round.
    // This function is called with the assumption that 1 or fewer tanks are currently active.
    private TankManager GetRoundWinner()
    {
        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if one of them is active, it is the winner so return it.
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        // If none of the tanks are active it is a draw so return null.
        return null;
    }


    // This function is to find out if there is a winner of the game.
    private TankManager GetGameWinner()
    {
        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if one of them has enough rounds to win the game, return it.
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        // If no tanks have enough rounds to win, return null.
        return null;
    }


    // Returns a string message to display at the end of each round.
    private string EndMessage()
    {
        // By default when a round ends there are no winners so the default end message is a draw.
        string message = "DRAW!";

        // If there is a winner then change the message to reflect that.
        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        // Add some line breaks after the initial message.
        message += "\n\n\n\n";

        // Go through all the tanks and add each of their scores to the message.
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        // If there is a game winner, change the entire message to reflect that.
        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    // This function is used to turn all the tanks back on and reset their positions and properties.
    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }

    private void ResetAllBots()
    {
        for (int i = 0; i < m_Bot1Count; i++)
        {
            m_Bots1[i].m_Instance.SetActive(false);
            m_Bots1[i].m_Instance = null;
        }
        m_Bot1Count = 0;

        for (int i = 0; i < m_Bot2Count; i++)
        {
            m_Bots2[i].m_Instance.SetActive(false);
            m_Bots2[i].m_Instance = null;
        }
        m_Bot2Count = 0;
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}
using UnityEngine;
using NaughtyAttributes;

public class OptometryPhaseManager : MonoBehaviour
{
    private Vector3 m_userCameraPosition;
    private GameObject m_firstPhaseHolder;
    private GameObject m_secondPhaseHolder;
    private GameObject[] m_tumplingEObjects;

    [Header("General Settings")]
    [SerializeField] private Transform cameraRig;
    [SerializeField] private GameObject tumblingEPrefab;
    [Tooltip("MainSize should be in centimeters.")]
    [SerializeField] [Min(0f)] private float tumblingEMainSize;
    [SerializeField] private bool activateAtFirst; // Will app starts as first phase is active?

    [Header("First Phase Settings")]
    [Tooltip("Distance should be in meters.")]
    [SerializeField] [Range(0.2f, 9f)] private float firstPhaseDistanceFromCamera;
    [Tooltip("Size should be in centimeters.")]
    [SerializeField] [Min(0f)] private float firstPhaseTumblingESize;

    [Header("Second Phase Settings")]
    [Tooltip("Distance should be in meters.")]
    [SerializeField] [Range(0.2f, 9f)] private float secondPhaseDistanceFromCamera;
    [Tooltip("Size should be in centimeters.")]
    [SerializeField] [Min(0f)] private float secondPhaseTumblingESize;
    [SerializeField] private bool rotateTumblingE;
    [SerializeField] private bool automaticPositioning;
    [Tooltip("Works if automatic positioning is on")]
    [ShowIf("automaticPositioning")]
    [SerializeField] [Min(0f)] private int tumblingECount;
    [ShowIf("automaticPositioning")]
    [SerializeField] private float angleBetween;
    [HideIf("automaticPositioning")]
    [SerializeField] private Vector2[] tumblingEPositions;
    [SerializeField] [Min(0.1f)] private float positioningRange;

    // Start is called before the first frame update
    private void Start()
    {
        Init();

        if (activateAtFirst)
            m_firstPhaseHolder.SetActive(true);
    }

    public void ToggleFirstPhase()
    {
        m_firstPhaseHolder.SetActive(!m_firstPhaseHolder.activeInHierarchy);
    }

    public void ToggleSecondPhase()
    {
        m_secondPhaseHolder.SetActive(!m_secondPhaseHolder.activeInHierarchy);
    }

    public void ToggleTumplingEObjects(int index)
    {
        int i = index - 1;
        if (i >= m_tumplingEObjects.Length) return;

        m_tumplingEObjects[i].SetActive(!m_tumplingEObjects[i].activeInHierarchy);
    }

    private void Init()
    {
        // Get user camera position
        m_userCameraPosition = cameraRig.position;

        // First phase configuration and setup
        m_firstPhaseHolder = new GameObject("First Phase Holder"); // Create first phase holder
        m_firstPhaseHolder.SetActive(false); // Hide the phase holder
        m_firstPhaseHolder.transform.position = m_userCameraPosition + (Vector3.forward * firstPhaseDistanceFromCamera); // Position the phase holder
        GameObject _tumblingE = Instantiate(tumblingEPrefab,
            m_firstPhaseHolder.transform.position, Quaternion.identity, m_firstPhaseHolder.transform); // Instantiate tumbling E as a child to phase holder
        _tumblingE.transform.localScale *= firstPhaseTumblingESize / tumblingEMainSize; // Set tumbling E size

        // Second phase configuration and setup
        m_secondPhaseHolder = new GameObject("Second Phase Holder"); // Create second phase holder
        m_secondPhaseHolder.SetActive(false); // Hide the phase holder
        m_secondPhaseHolder.transform.position = m_userCameraPosition + (Vector3.forward * secondPhaseDistanceFromCamera); // Position the phase holder

        m_tumplingEObjects = new GameObject[tumblingECount];
        for (int i = 0; i < tumblingECount; i++) // Create and position (tumblingECount) tumblingE
        {
            _tumblingE = Instantiate(tumblingEPrefab,
            m_secondPhaseHolder.transform.position, Quaternion.identity, m_secondPhaseHolder.transform); // Instantiate tumbling E as a child to phase holder
            m_tumplingEObjects[i] = _tumblingE;
            _tumblingE.SetActive(false); // Deactive tumbling E
            _tumblingE.transform.localScale *= secondPhaseTumblingESize / tumblingEMainSize; // Set tumbling E size

            if (!automaticPositioning) // If automatic positioning is off, position tumblingE based on tumblingEPositions Array
                _tumblingE.transform.position += (Vector3) tumblingEPositions[i] * positioningRange;
            else // Else position mathematically
            {
                if (i == tumblingECount - 1) return;
                
                float _angle;
                if (angleBetween == 0)
                    _angle = (i * (360f / (tumblingECount - 1))) * Mathf.Deg2Rad;
                else
                    _angle = (i * angleBetween) * Mathf.Deg2Rad;

                    Vector2 _dir = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));
                _tumblingE.transform.position += (Vector3) _dir * positioningRange;
            }

            if (rotateTumblingE) // Rotate the tumblingE randomly in 90 degree variantes
            {
                _tumblingE.transform.localEulerAngles = Vector3.forward * Random.Range(0,4) * 90f;
            }
        }
    }
}

using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private float lightSpeed;
    [SerializeField] private float batteryEnduranceTime;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D flashlightLight;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D spotPlayerLight;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D globalLight;
    [SerializeField] private ClassicProgressBar progressBar;
    [SerializeField] private PlayerControllerLevel1 playerController;
    public bool isOneSide;
    private float lastTimer = 0f;
    private float currentTime;
    private float flashlightLightStartIntensity, spotPlayerLightStartIntensity, globalLightStartIntensity;

    [SerializeField] private int lightLimesUp, lightLimesDown;

    void Start()
    {
        progressBar.m_FillAmount = 1f;
        flashlightLightStartIntensity = flashlightLight.intensity;
        spotPlayerLightStartIntensity = spotPlayerLight.intensity;
        globalLightStartIntensity = globalLight.intensity;
    }

    void Update()
    {
       if (GameManager.instance.currentGameState == GameState.GS_GAME)
       {
            SetLightRotation();
            HandleTimeFlow();
       }
    }

    private void SetLightRotation()
    {
        var rot = gameObject.transform.localEulerAngles;

        if (!isOneSide)
        {
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && rot.z <= 180)
                rot.z = 360 - rot.z;
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && rot.z >= 180)
                rot.z = 360 - rot.z;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (rot.z >= lightLimesUp + 180 && rot.z <= lightLimesDown + 180)
            {
                rot.z += lightSpeed * Time.deltaTime;
                if (rot.z > lightLimesDown + 180) rot.z = lightLimesDown + 180;
            }
            else if(rot.z <= lightLimesDown && rot.z >= lightLimesUp)
            {
                rot.z -= lightSpeed * Time.deltaTime;
                if (rot.z < lightLimesUp) rot.z = lightLimesUp;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (rot.z >= lightLimesUp + 180 && rot.z <= lightLimesDown + 180)
            {
                rot.z -= lightSpeed * Time.deltaTime;
                if (rot.z < lightLimesUp + 180) rot.z = lightLimesUp + 180;
            }
            else if(rot.z <= lightLimesDown && rot.z >= lightLimesUp)
            {
                rot.z += lightSpeed * Time.deltaTime;
                if (rot.z > lightLimesDown) rot.z = lightLimesDown;
            }
        }

        rot.z %= 360;
        gameObject.transform.localEulerAngles = rot;
    }

    private void HandleTimeFlow()
    {
        currentTime = GameManager.instance.GetTimer();
        var batteryPercent = 1f - ((currentTime - lastTimer)/batteryEnduranceTime);
        if(batteryPercent <= 0) 
        {
            playerController.KillPlayer();
            return;
        }
        progressBar.m_FillAmount = batteryPercent;
        flashlightLight.intensity = batteryPercent * flashlightLightStartIntensity;
        spotPlayerLight.intensity = batteryPercent * spotPlayerLightStartIntensity;
        globalLight.intensity = batteryPercent * globalLightStartIntensity;
    }

    public void BatteryTakenEvent()
    {
        lastTimer = currentTime;
    }
}

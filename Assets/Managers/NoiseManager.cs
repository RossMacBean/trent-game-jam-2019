using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    // Singleton instance
    private static NoiseManager mNoiseManager;

    public Slider noiseSlider;
    public Image sliderImage;
    public float maxNoise = 100.0f;
    public float startingNoise = 0.0f;
    public float standardDecayRate = 0.2f;
    public float standardDecayDelay = 2.0f;
    public float alertDecayRate = 0.15f;
    public float alertDecayDelay = 5.0f;
    public bool isAlertMode = false;

    public bool IsMax => currentNoiseLevel == maxNoise;
    public bool IsMin => currentNoiseLevel == 0.0f;

    private Color initialSliderColor;

    private float currentNoiseLevel = 0.0f;
    private float noiseChange = 0.0f;
    private float timeSinceLastIncrease = 0.0f;

    public static NoiseManager Instance
    {
        get { return mNoiseManager; }
    }

    void Awake()
    {
        if (!mNoiseManager)
        {
            mNoiseManager = this;
        }
        else if (mNoiseManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sliderImage = noiseSlider.transform.Find("Fill Area").GetComponentInChildren<Image>();
        initialSliderColor = sliderImage.color;

        currentNoiseLevel = isAlertMode ? maxNoise : Mathf.Min(startingNoise, maxNoise);
    }

    void Update()
    {
        UpdateNoiseLevel();
        UpdateSlider();
    }

    public void IncreaseNoise(float f)
    {
        noiseChange += f;
    }

    public void ReduceNoise(float f)
    {
        noiseChange -= f;
    }

    private void UpdateNoiseLevel()
    {
        float previousNoiseLevel = currentNoiseLevel;
        bool hasNoiseLevelIncreased = noiseChange > 0;

        if (hasNoiseLevelIncreased)
        {
            timeSinceLastIncrease = 0.0f;
        }
        else
        {
            timeSinceLastIncrease += Time.deltaTime;
        }

        float overallChange = noiseChange;

        float decayDelay = isAlertMode ? alertDecayDelay : standardDecayDelay;
        if (timeSinceLastIncrease >= decayDelay)
        {
            float decayRate = isAlertMode ? alertDecayRate : standardDecayRate;
            overallChange -= decayRate;
        }

        currentNoiseLevel = Mathf.Clamp(currentNoiseLevel + overallChange, 0, maxNoise);

        if (IsMax)
        {
            isAlertMode = true;
        }

        if (IsMin)
        {
            isAlertMode = false;
        }

        noiseChange = 0.0f;
    }

    private void UpdateSlider()
    {
        if (!noiseSlider)
            return;

        noiseSlider.value = Mathf.Min(currentNoiseLevel, maxNoise);
        sliderImage.color = isAlertMode ? Color.red : initialSliderColor;
    }
}

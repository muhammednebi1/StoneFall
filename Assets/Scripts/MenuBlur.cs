using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MenuBlur : MonoBehaviour
{
    GameManager gameManager;
    Volume volume;
    VolumeProfile volumeProfile;
    DepthOfField depthOfField;
    ColorAdjustments adjustments;
    float hueShiftValue = 0;

    void BackgroundBlur()
    {
        if (gameManager.gamePaused ||  gameManager.gameStarted || gameManager.gameOver)
        {
            depthOfField.focusDistance.value = 0.1f;
            hueShiftValue += Time.deltaTime * 20;
            if (hueShiftValue >= 180)
            {
                hueShiftValue = -180;
            }
            adjustments.hueShift.value = hueShiftValue;
        }
        else
        {
            depthOfField.focusDistance.value = 10;
            hueShiftValue = 0;
            adjustments.hueShift.value = hueShiftValue;
        }
    }

    void GameOverEffect()
    {
        if (gameManager.gameOver)
        {
            adjustments.saturation.value = -100;
        }
        else
        {
            adjustments.saturation.value = 100;
        }
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        volume = GetComponent<Volume>();
        volumeProfile = volume.profile;
        volumeProfile.TryGet(out depthOfField);
        volumeProfile.TryGet(out adjustments);
    }

    void Update()
    {
        BackgroundBlur();
        GameOverEffect();
    }
}

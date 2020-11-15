using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Created by Marcelino Borges.
/// 
/// Light blinking
/// With fine tuning of blink interval and
/// intensity of the light at a low brightness (actual blink)
/// </summary>
public class TorchLight : MonoBehaviour {
    #region PROPERTIES
    [Header("DROP REFERENCE")]
    public Light2D torchLight;
    [Header("FINE TUNING")]
    [MinMaxSlider]
    [Tooltip("The time (in seconds) the light will stay at the lower intensity. Each iteration picks a random time between the 2 bounds set in the slider.")]
    public Vector2 blinkInterval; //How long the light keeps at low intensity (x: min value, y: max value)

    [MinMaxSlider(0.5f,.99f)]
    [Tooltip("The low value (randomly picked between the 2 values in the slider) to be set as the light intensity at each blink iteration.")]
    public Vector2 lowIntensityBounds; //when the light blinks, it will be this value of low intensity that will control be seen by player (x: min value, y: mas value)
    #endregion

    #region METHODS
    private void Start() {
        //Exception if starts the game without the light reference
        if (torchLight == null)
            throw new MissingReferenceException("Referenciar a luz 2D da torcha.");
        else
            //Calling the Blink method
            StartCoroutine(BlinkRepeatingCo());
    }

    IEnumerator BlinkRepeatingCo() {
        //Sets a random intensity value inside the limits set in inspector for lowIntensityBounds
        torchLight.intensity = Random.Range(lowIntensityBounds.x, lowIntensityBounds.y);
        //Waits a random time inside the limits set in inspector for blinkInterval
        yield return new WaitForSeconds(Random.Range(blinkInterval.x, blinkInterval.y));
        //Sets the light intensity to the normal value (1)
        torchLight.intensity = 1;
        //Recursively calling this function again 
        StartCoroutine(BlinkRepeatingCo());
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InferenceSettings : MonoBehaviour
{
    public TMPro.TMP_InputField testerName;
    public TMPro.TMP_Dropdown testMode;
    public Button playButton;
    public InferenceEngine engine;

    public int gameMode = 0;

    public void playGame()
    {
        gameMode = testMode.value;
        engine.inferences[gameMode].SetActive(true);
        engine.inferences[gameMode].GetComponent<GameController>().inference_id = testerName.text;        
        gameObject.SetActive(false);
    }


}

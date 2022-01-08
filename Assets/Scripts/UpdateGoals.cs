using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGoals : MonoBehaviour
{
    private Text text;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        text = GetComponent<Text>();
        text.text = gameManager.getGoalsAsString();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.WinCondition())
        {
            text.text = "WINNNNNN";
        }
        else
        {
            text.text = gameManager.getGoalsAsString();
        }
    }
}

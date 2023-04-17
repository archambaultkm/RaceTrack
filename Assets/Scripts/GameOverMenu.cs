using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ToggleMenu(false);
    }

    public void ToggleMenu(bool active)
    {
        gameObject.SetActive(active);

        if (active)
            Time.timeScale = 0; //pause the game so the "car" doesn't fall forever
        else
            Time.timeScale = 1;
    }
}

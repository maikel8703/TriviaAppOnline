using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public string level;
    public void ComenzarJuego()
    {

        SceneManager.LoadScene(level);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

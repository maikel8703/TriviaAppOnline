using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
   public static Gamemanager instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            
            Destroy(gameObject);
            //instance=this;
        }

        
    }

    public void ChangeScene(int _sceneIndex)
    {
        SceneManager.LoadSceneAsync(_sceneIndex);

    }
}

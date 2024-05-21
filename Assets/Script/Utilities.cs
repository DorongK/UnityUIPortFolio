using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utilities 
{
    public static int PlayerDeaths = 0;
    public static void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
    public static bool RestartLevel(int sceneidx)
    {
        Debug.Log("Player deaths:" + PlayerDeaths);
        string message = UpdateDeathCount(ref PlayerDeaths);
        Debug.Log("Player deaths:" + PlayerDeaths);
        Debug.Log(message);

        SceneManager.LoadScene(sceneidx);
        Time.timeScale = 1.0f;
        
        return true;
    }
    public static string UpdateDeathCount(ref int countReference)
    {
        countReference += 1;
        return "Next time you'll e at number" + countReference;
    }

}

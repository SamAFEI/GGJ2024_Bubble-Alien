/*
    璸だ
    ﹀秖
    ち传初春
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int pointsA; //Aだ计
    public static int pointsB; //Bだ计
    public static int hp;  //差﹀秖

    public Text pointsAText; //陪ボAだ计Text
    public Text pointsBText; //Bだ计Text
    public Text hpText;  //差﹀秖Text

    private void Start()
    {
        ShowPoints();
    }

    public void ShowPoints()
    {
        pointsAText.text = "A Points" + pointsA;
        pointsBText.text = "B Points" + pointsB;
        hpText.text = "HP" + hp;
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Cosmic");
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

}

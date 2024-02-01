/*
    �p��
    ��q
    ��������
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int pointsA; //A������
    public static int pointsB; //B������
    public static int hp;  //���q

    public Text pointsAText; //���A���ƪ�Text
    public Text pointsBText; //B���ƪ�Text
    public Text hpText;  //���q��Text

    private void Start()
    {
        ShowPoints();
    }

    public void ShowPoints()
    {
        pointsAText.text = "A Points�G" + pointsA;
        pointsBText.text = "B Points�G" + pointsB;
        hpText.text = "HP�G" + hp;
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

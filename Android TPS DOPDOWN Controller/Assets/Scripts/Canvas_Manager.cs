using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Canvas_Manager : MonoBehaviour
{
    [SerializeField] TMP_Text text_dir;
    [SerializeField] Image health_bar;  


    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void Repaint_Health_Value(float health, float max_health)
    {
        health_bar.fillAmount = health / max_health;
    }

    public void Restart_Scene()
    {
        SceneManager.LoadScene(0);
    }

    #region move text
    public void Move_Forward() { text_dir.text = "MOVE FORWARD"; }
    public void Move_Right() { text_dir.text = "MOVE RIGHT"; }
    public void Move_Back() { text_dir.text = "MOVE BACK"; }
    public void Move_Left() { text_dir.text = "MOVE LEFT"; }
    #endregion
}
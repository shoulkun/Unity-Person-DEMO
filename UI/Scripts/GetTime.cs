using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetTime: MonoBehaviour
{
    private EnemiesManager enemiesManager;
    /// <summary>
    /// 局内计时器
    /// </summary>
    private float nowTime;
    private TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {
        enemiesManager = EnemiesManager.Instance;
        Text = transform.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        nowTime = enemiesManager.difficultyTime;
        Text.text = ((int)nowTime).ToString();
    }
}

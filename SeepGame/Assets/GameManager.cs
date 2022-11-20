using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField, Header("生成のタイミング")] float createTime = 2f;
    [SerializeField, Tooltip("生成するオブジェクト")] GameObject seepObj = default;
    SeepController seepController;
    int score = 0;
    int combo = 0;
    [SerializeField] Text text;
    [SerializeField] Text timeText;

    bool isGame = true;

    
    void Update()
    {
        if(isGame)
        {
            if(time <= 0)
            {
                isGame = false;
            }
            createTime -= Time.deltaTime;
            time -= Time.deltaTime;
            if (createTime <= 0)
            {
                var seep = Instantiate(seepObj, transform.position, seepObj.transform.rotation);
                seepController = seep.GetComponent<SeepController>();
                var rand = Random.Range(0, 101);
                if (rand >= 0 && rand < 50)
                {
                    seepController.SetKind(SeepController.Kind.Normal);
                }
                else if (rand >= 50 && rand < 70)
                {
                    seepController.SetKind(SeepController.Kind.Slow);
                }
                else if (rand >= 70 && rand < 90)
                {
                    seepController.SetKind(SeepController.Kind.Fast);
                }
                else if (rand >= 90 && rand < 101)
                {
                    seepController.SetKind(SeepController.Kind.Bound);
                }
                createTime = 2f;
            }

        }
        else
        {
            if(score <= 10)
            {
                Debug.Log("悪夢");
            }
            else if(score <= 25)
            {
                Debug.Log("良眠");
            }
            else
            {
                Debug.Log("快眠");
            }
        }

        text.text = $"{score}匹  {combo}コンボ";
        timeText.text = time.ToString("0");
    }

    public void AddScore()
    {
        score++;
    }

    public void AddCombo(bool hit)
    {
        if(hit)
        {
            combo++;
        }
        else
        {
            score += 2 * combo;
            combo = 0;
        }
    }
}

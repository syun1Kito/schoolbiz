using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour {


    private SpriteRenderer gauge;
    private float maxhp;

	// Use this for initialization
	void Start () {
        gauge= GetComponent<SpriteRenderer>();//敵のHPゲージ
        maxhp = EnemyController.hp;//HP最大値

    }

    // Update is called once per frame
    void Update()
    {
        //----------------------　敵HPによって色が緑→黄→赤と変化
        if (EnemyController.hp >= maxhp / 2)
        {
            gauge.color = new Color(1 - (2 * EnemyController.hp - maxhp) / maxhp, 1, 0, 255 / 255f);
        }

        if (EnemyController.hp < maxhp / 2)
        {
            gauge.color = new Color(1, 2 * EnemyController.hp / maxhp, 0, 255 / 255f);
        }

    }
}

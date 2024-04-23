using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myd.Platform
{
    public class SwtichAnim : MonoBehaviour  // 确保定义了一个类
    {
        private Animator anim;
        private Rigidbody2D rb;

        [SerializeField]
        GameObject Sprite;

        public void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = Sprite.GetComponent<Animator>();
        }

        public void Update()
        {
            
            SwitchAnim();
        }

        public void SwitchAnim()
        {
            anim.SetFloat("running", Mathf.Abs(Player.Instance.playerController.Speed.x));
            anim.SetBool("falling", false);

            if (Player.Instance.playerController.Speed.y > 0)
            {
                anim.SetBool("jumping", true);
            }
            else if (Player.Instance.playerController.Speed.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
    }
}

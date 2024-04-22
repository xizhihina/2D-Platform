using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myd.Platform
{
    public class ElevatorController : MonoBehaviour
    {
        public float Speed=1f;
        private float TopY, BottomY;
        public float upHeight=3f;

        private bool isUp = true;

        // Start is called before the first frame update
        void Start()
        {
            TopY = transform.position.y + upHeight; //获取top点
            BottomY = transform.position.y; //获取bottom点
        }

        // Update is called once per frame
        void Update()
        {
            Movement();
        }

        void Movement()
        {
            if (isUp)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + Speed * Time.deltaTime);
                //判断Player单例是否存在，PlayerController是否存在
                if (Player.Instance != null && Player.Instance.playerController != null)
                {
                    //检测是否脚下碰撞到了Elevator
                    Collider2D collideCheck=Player.Instance.playerController.CollideCheck(Player.Instance.playerController.Position, Vector2.down);
                    if(collideCheck&&collideCheck.gameObject.name=="Elevator")
                    {
                        //Player的位置随着Elevator的上升而上升
                        Player.Instance.playerController.Position += Vector2.up * (Speed * Time.deltaTime);
                    }
                }
                if (transform.position.y >= TopY)
                {
                    isUp = false;
                }
            }
            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - Speed * Time.deltaTime);
                if (transform.position.y <= BottomY)
                {
                    isUp = true;
                }
            }
        }
    }
}
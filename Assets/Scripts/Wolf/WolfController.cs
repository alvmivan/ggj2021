using System;
using Fire;
using Gameplay;
using UniRx;
using UnityEngine;
using UnityEngine.Animations;

namespace Wolf
{
    public class WolfController : MonoBehaviour
    {
        [Header("Settings")] public float speed = 1;
        public float rotationSpeed = 45f;
        public CharacterController characterController;
        public Transform stickHolder;


        #if UNITY_EDITOR
        private float cheatSpeed = 10;
        #else
        private float cheatSpeed = 1;
        #endif
        
        //variables
        Vector2 axesInput;
        private Stick stick;

        public readonly ReactiveProperty<float> currentHeath = new ReactiveProperty<float>(1);

        private void Awake()
        {
            currentHeath.Value = 1;
        }

        private void Update()
        {
            Walk();
            UpdateStick();
        }

        private void UpdateStick()
        {
            if (stick)
            {
                stick.stickRender.SetPositionAndRotation(stickHolder.position, stickHolder.rotation);
            }
        }

        private void Walk()
        {
            axesInput = new Vector2(Input.GetAxis("Horizontal"), Mathf.Max(Input.GetAxis("Vertical"),0));

            if (GlobalProperties.PlayerInputBlocked.Value) return;

            var cheat = Input.GetKey(KeyCode.LeftShift)?cheatSpeed:1;
            characterController.SimpleMove(characterController.transform.forward * (speed * axesInput.y * cheat));

        }

        private void FixedUpdate()
        {
            characterController.transform.Rotate(0, rotationSpeed * axesInput.x * Time.fixedDeltaTime, 0);
        }

        public void GrabStick(Stick s)
        {
            if(stick) 
                return;
            stick = s;
        }

        public Stick DropStick()
        {
            var ret = stick;
            if (stick)
            {
                stick.OnDrop();
            }
            stick = null;
            return ret;
        }

        public void SetHeath(int fireLevel)
        {
            currentHeath.Value = Mathf.Max(fireLevel, currentHeath.Value);
        }
    }
}
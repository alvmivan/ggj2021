using System;
using Fire;
using Gameplay;
using Injector.Core;
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

        public float[] heatLevels = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};


#if UNITY_EDITOR
        private float cheatSpeed = 4;
#else
        private float cheatSpeed = 1;
#endif

        private bool nearFire = false;
        
        //variables
        Vector2 axesInput;
        private Stick stick;
        float cheat = 1;

        public readonly ReactiveProperty<float> currentHeat = new ReactiveProperty<float>(1);

        private void Awake()
        {
            Injection.Register(this);
            currentHeat.Value = 1;
        }

        private void Update()
        {
            Walk();
            UpdateStick();
            UpdateHeat();
        }

        private void UpdateHeat()
        {
            const float coldness = 1;
            if (nearFire)
            {
                return;
            }
            currentHeat.Value -= Time.deltaTime * coldness;
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
            const string horizontal = "Horizontal";
            const string vertical = "Vertical";

            axesInput = new Vector2(Input.GetAxis(horizontal), Mathf.Max(Input.GetAxis(vertical), 0));

            if (GlobalProperties.PlayerInputBlocked.Value) return;

            cheat = Input.GetKey(KeyCode.LeftShift) ? cheatSpeed : 1;

            characterController.SimpleMove(characterController.transform.forward * (speed * axesInput.y * cheat));
        }

        private void FixedUpdate()
        {
            characterController.transform.Rotate(0, rotationSpeed * axesInput.x * Time.fixedDeltaTime * cheat, 0);
        }

        public void GrabStick(Stick s)
        {
            if (stick)
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
            currentHeat.Value = Mathf.Max(heatLevels[fireLevel], currentHeat.Value);
        }

        public void SetNearFire(bool near)
        {
            nearFire = near;
        }
    }
}
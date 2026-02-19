using Character;
using UnityEngine;

namespace Character {
    public class AnimatedController : MonoBehaviour
    {
        [SerializeField] float moveSpeed; // useful to observe for debugging
        [SerializeField] GameObject wateringCan;
        MovementController moveController;
        Animator animator;
        protected Animator Animator { get { return animator; } }
        void Start()
        {
            animator = GetComponent<Animator>();
            moveController = GetComponent<MovementController>();
            wateringCan.SetActive(false);
        }

        public void SetTrigger(string name)
        {
            animator.SetTrigger(name);
        }

        public void SetTool(string tool)
        {
            if (tool == "WateringCan")
            {
                wateringCan.SetActive(true);
            }
            else if (tool == "None")
            {
                wateringCan.SetActive(false);
            }
        }

        void Update()
        {
            moveSpeed = moveController.GetHorizontalSpeedPercent();
            animator.SetFloat("Speed", moveSpeed);
        }
    }
}

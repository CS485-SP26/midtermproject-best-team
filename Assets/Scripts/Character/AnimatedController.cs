using UnityEngine;

namespace Character {
    public class AnimatedController : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        MovementController moveController;
        Animator animator;
        protected Animator Animator { get { return animator; } }

        private bool isCelebrating = false;

        void Awake()
        {
            animator = GetComponent<Animator>();
            moveController = GetComponent<MovementController>();
        }

        public void SetTrigger(string name)
        {
            animator.SetTrigger(name);
            if (name == "Celebrate")
            {
                isCelebrating = true;
                Invoke(nameof(StopCelebrating), 3f); // match length of dance animation
            }
        }

        private void StopCelebrating()
        {
            isCelebrating = false;
        }

        void Update()
        {
            if (isCelebrating) return; // don't update speed during dance
            moveSpeed = moveController.GetHorizontalSpeedPercent();
            animator.SetFloat("Speed", moveSpeed);
        }
    }
}
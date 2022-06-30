using UnityEngine;

public class EnemyAnimation : StateMachineBehaviour {
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("Attack", false);
    }
}

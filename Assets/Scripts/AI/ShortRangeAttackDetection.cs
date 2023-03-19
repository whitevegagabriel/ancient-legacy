using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public class ShortRangeAttackDetection : StateMachineBehaviour
    {
        private static List<AttackCallback> _attackCallbacks = new List<AttackCallback>();
        private int _currLoopIteration;
        private bool _calledPartway;
        
        public static void AddAttackCallback(AttackCallback callback)
        {
            _attackCallbacks.Add(callback);
        }
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _currLoopIteration = -1;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Mathf.FloorToInt(stateInfo.normalizedTime) > _currLoopIteration)
            {
                _currLoopIteration++;
                _calledPartway = false;
            }
            
            if (stateInfo.normalizedTime >= _currLoopIteration + 0.2f && !_calledPartway)
            {
                _calledPartway = true;
                foreach (var callback in _attackCallbacks)
                {
                    callback.onAttackPartway.Invoke();
                }
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var callback in _attackCallbacks)
            {
                callback.onAttackEnd.Invoke();
            }   
        }
    }

    public struct AttackCallback
    {
        public UnityAction onAttackPartway;
        public UnityAction onAttackEnd;
    
        public AttackCallback(UnityAction onAttackPartway, UnityAction onAttackEnd)
        {
            this.onAttackPartway = onAttackPartway;
            this.onAttackEnd = onAttackEnd;
        }
    }
}
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
 
public class AnimUnhideTool : ScriptableObject
{
    [MenuItem("Assets/Unhide Fix")]
    private static void unhide()
    {
        //UnityEditor.Animations.
        AnimatorController ac = Selection.activeObject as AnimatorController;
        foreach (AnimatorControllerLayer layer in ac.layers)
        {
            foreach(ChildAnimatorStateMachine stateMachine in layer.stateMachine.stateMachines)
            {
                unhideXun(stateMachine);
                if (stateMachine.stateMachine.hideFlags != 0)
                {
                    stateMachine.stateMachine.hideFlags = 0;
                }
            }
            foreach (ChildAnimatorState curState in layer.stateMachine.states)
            {
                if (curState.state.hideFlags != 0)
                {
                    curState.state.hideFlags = 0;
                    //Debug.Log(curState.state.ToString());
                }
                foreach (AnimatorStateTransition animatorStateTransition in curState.state.transitions)
                {
 
                    if (animatorStateTransition.hideFlags != 0)
                    {
                        animatorStateTransition.hideFlags = 0;
                        // Debug.Log(animatorStateTransition.ToString());
                    }
                }
            }          
        }
        EditorUtility.SetDirty(ac);
    }
    static void unhideXun(ChildAnimatorStateMachine a)
    {
        foreach (ChildAnimatorState curState in a.stateMachine.states)
        {
            if (curState.state.hideFlags != 0)
            {
                curState.state.hideFlags = 0;
                //Debug.Log(curState.state.ToString());
            }
            foreach (AnimatorStateTransition animatorStateTransition in curState.state.transitions)
            {
 
 
                if (animatorStateTransition.hideFlags != 0)
                {
                    animatorStateTransition.hideFlags = 0;
                    // Debug.Log(animatorStateTransition.ToString());
                }
            }
        }
        foreach (ChildAnimatorStateMachine stateMachine in a.stateMachine.stateMachines)
        {
            unhideXun(stateMachine);
            if (stateMachine.stateMachine.hideFlags != 0)
            {
                stateMachine.stateMachine.hideFlags = 0;
            }
        }
    }
}
#endif
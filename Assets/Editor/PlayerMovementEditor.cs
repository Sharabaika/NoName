using JetBrains.Annotations;
using Player;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PlayerMovement))]
    public class PlayerMovementEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            string stateLabel = "None";
            if (((PlayerMovement) target)?.CurrentState != null)
            {
                stateLabel = ((PlayerMovement) target).CurrentState.Type.ToString();
            }
            EditorGUILayout.LabelField("State", stateLabel);

        }
    }
}
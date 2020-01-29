using UnityEngine;
using UnityEditor;
namespace Scripts.UpdateMoney.Editor
{
    [CustomEditor(typeof(UpdateMoney))]
    public class UpdateMoneyEditor : UnityEditor.Editor
    {
        /// <inheritdoc />
        /// <summary>
        /// on inspector gui.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var updateMoney = (UpdateMoney)target;
            if (!GUILayout.Button("Update Money")) return;
            if (!string.IsNullOrEmpty(updateMoney.UserRegisteredEmail) &&
                !string.IsNullOrEmpty(updateMoney.UserUpdatedMoney))
            {
                updateMoney.UpdateUserMoney(updateMoney.UserRegisteredEmail, updateMoney.UserUpdatedMoney);
                updateMoney.OnUpdateComplete += updateMoney.ShowOutPut;
                Debug.ClearDeveloperConsole();
                EditorUtility.DisplayDialog("Message:", "Check Console for details.", "okay");
            }
            else
            {


                EditorUtility.DisplayDialog("Filed: ","Value of email or money is not set.","okay");
            }
        }
    }
}

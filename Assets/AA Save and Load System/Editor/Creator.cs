// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using UnityEngine;
using UnityEditor;

namespace AASave
{
    public static class AASLCreator
    {
        private static GameObject SaveSystemObject = null;

        [MenuItem("GameObject/AA Save and Load System/Create Save System", false, 0)]
        public static void CreateSaveSystemMenu()
        {
            SaveSystemObject = new GameObject("Save System");
            SaveSystemObject.transform.SetPositionAndRotation(new Vector3(0F, 0F, 0F), Quaternion.Euler(0F, 0F, 0F));
            SaveSystemObject.AddComponent<SaveSystem>();
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(SaveSystemObject.GetComponent<SaveSystem>(), true);

            Selection.activeGameObject = SaveSystemObject;
        }

    }
}

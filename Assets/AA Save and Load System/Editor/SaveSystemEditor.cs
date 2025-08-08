// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using UnityEngine;
using UnityEditor;

namespace AASave
{
    [CustomEditor(typeof(SaveSystem))]
    public class SaveSystemEditor : Editor
    {
        [Tooltip("This Rect instance is used to get the Rect of the Inspector fields.")] private Rect typeRect;

        private readonly string consoleLogPrefix = "<color=#3de32d>AA Save and Load System : </color>";
        private bool _encryptionKeyRandomized = false;

        public override void OnInspectorGUI()
        {
            SaveSystem saveSystem = (SaveSystem)target;

            saveSystem.dontDestroyOnLoad = EditorGUILayout.Toggle("Don't Destroy on Load", saveSystem.dontDestroyOnLoad);
            typeRect = GUILayoutUtility.GetLastRect();
            GUI.Label(typeRect, new GUIContent("", "If this is checked, this component and the GameObject will not be destroyed while loading a new scene.\n\nThis is completely optional."));

            GUILayout.Space(20F);

            GUILayout.Label("Save File Location");
            saveSystem.fileLocation = (AASave.FileLocations)EditorGUILayout.EnumPopup("File Location", saveSystem.fileLocation);
            typeRect = GUILayoutUtility.GetLastRect();
            GUI.Label(typeRect, new GUIContent("", "The file path where the save files will be located.\n\nPersistent Data Path: The path to a persistent data directory. (Recommended)\n\nApplication Data Path: The path to the game data folder on the target device.\n\nTemporary Cache Path: The path to a temporary data / cache directory.\n\nStreaming Assets Path: The path to the StreamingAssets folder.\n\nCustom Path: A custom file location. Enter full path."));

            if (saveSystem.fileLocation == AASave.FileLocations.CustomPath)
            {
                saveSystem.customFilePath = EditorGUILayout.TextField("Custom Path", saveSystem.customFilePath);
                typeRect = GUILayoutUtility.GetLastRect();
                GUI.Label(typeRect, new GUIContent("", "Custom save file location. Please enter the full path.\n\nExample: C:\\Users\\UserName\\...\\ProjectName\\..."));
            }

            saveSystem.subFolder = EditorGUILayout.Toggle("Sub Folder", saveSystem.subFolder);
            typeRect = GUILayoutUtility.GetLastRect();
            GUI.Label(typeRect, new GUIContent("", "If this is checked, sub folders can be created on the target save location to store the save files."));

            if (saveSystem.subFolder)
            {
                saveSystem.subFolderName = EditorGUILayout.TextField("Folder Name", saveSystem.subFolderName);
                typeRect = GUILayoutUtility.GetLastRect();
                GUI.Label(typeRect, new GUIContent("", "Name of the sub folder."));
            }

            if (GUILayout.Button("Open Save Location"))
            {
                if (saveSystem.fileLocation == AASave.FileLocations.PersistentDataPath)
                {
                    if (saveSystem.subFolder)
                    {
                        if (!string.IsNullOrEmpty(saveSystem.subFolderName) && !string.IsNullOrWhiteSpace(saveSystem.subFolderName))
                        {
                            if (!System.IO.Directory.Exists(Application.persistentDataPath + "/" + saveSystem.subFolderName))
                            {
                                System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + saveSystem.subFolderName);
                            }

                            System.Diagnostics.Process.Start(Application.persistentDataPath + "/" + saveSystem.subFolderName);
                        }
                        else
                        {
                            Debug.LogError(consoleLogPrefix + "Sub folder name cannot be empty or white space.\n");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(Application.persistentDataPath);
                    }
                }
                else if (saveSystem.fileLocation == AASave.FileLocations.ApplicationDataPath)
                {
                    if (saveSystem.subFolder)
                    {
                        if (!string.IsNullOrEmpty(saveSystem.subFolderName) && !string.IsNullOrWhiteSpace(saveSystem.subFolderName))
                        {
                            if (!System.IO.Directory.Exists(Application.dataPath + "/" + saveSystem.subFolderName))
                            {
                                System.IO.Directory.CreateDirectory(Application.dataPath + "/" + saveSystem.subFolderName);
                            }

                            System.Diagnostics.Process.Start(Application.dataPath + "/" + saveSystem.subFolderName);
                        }
                        else
                        {
                            Debug.LogError(consoleLogPrefix + "Sub folder name cannot be empty or white space.\n");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(Application.dataPath);
                    }
                }
                else if (saveSystem.fileLocation == AASave.FileLocations.TemporaryCachePath)
                {
                    if (saveSystem.subFolder)
                    {
                        if (!string.IsNullOrEmpty(saveSystem.subFolderName) && !string.IsNullOrWhiteSpace(saveSystem.subFolderName))
                        {
                            if (!System.IO.Directory.Exists(Application.temporaryCachePath + "/" + saveSystem.subFolderName))
                            {
                                System.IO.Directory.CreateDirectory(Application.temporaryCachePath + "/" + saveSystem.subFolderName);
                            }

                            System.Diagnostics.Process.Start(Application.temporaryCachePath + "/" + saveSystem.subFolderName);
                        }
                        else
                        {
                            Debug.LogError(consoleLogPrefix + "Sub folder name cannot be empty or white space.\n");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(Application.temporaryCachePath);
                    }
                }
                else if (saveSystem.fileLocation == AASave.FileLocations.StreamingAssetsPath)
                {
                    try
                    {
                        if (saveSystem.subFolder)
                        {
                            if (!string.IsNullOrEmpty(saveSystem.subFolderName) && !string.IsNullOrWhiteSpace(saveSystem.subFolderName))
                            {
                                if (!System.IO.Directory.Exists(Application.streamingAssetsPath + "/" + saveSystem.subFolderName))
                                {
                                    System.IO.Directory.CreateDirectory(Application.streamingAssetsPath + "/" + saveSystem.subFolderName);
                                }

                                System.Diagnostics.Process.Start(Application.streamingAssetsPath + "/" + saveSystem.subFolderName);
                            }
                            else
                            {
                                Debug.LogError(consoleLogPrefix + "Sub folder name cannot be empty or white space.\n");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(Application.streamingAssetsPath);
                        }
                    }
                    catch
                    {
                        Debug.LogWarning(consoleLogPrefix + "File location could not be found: " + Application.streamingAssetsPath + "\n");
                    }
                }
                else if (saveSystem.fileLocation == AASave.FileLocations.CustomPath)
                {
                    try
                    {
                        if (saveSystem.subFolder)
                        {
                            if (!string.IsNullOrEmpty(saveSystem.subFolderName) && !string.IsNullOrWhiteSpace(saveSystem.subFolderName))
                            {
                                if (!System.IO.Directory.Exists(saveSystem.customFilePath + "/" + saveSystem.subFolderName))
                                {
                                    System.IO.Directory.CreateDirectory(saveSystem.customFilePath + "/" + saveSystem.subFolderName);
                                }

                                System.Diagnostics.Process.Start(saveSystem.customFilePath + "/" + saveSystem.subFolderName);
                            }
                            else
                            {
                                Debug.LogError(consoleLogPrefix + "Sub folder name cannot be empty or white space.\n");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(saveSystem.customFilePath);
                        }
                    }
                    catch
                    {
                        Debug.LogWarning(consoleLogPrefix + "File location could not be found: " + saveSystem.customFilePath + "\n");
                    }
                }
            }

            GUILayout.Space(20F);

            GUILayout.Label("Save File Extension");
            saveSystem.fileExtension = EditorGUILayout.TextField("File Extension", saveSystem.fileExtension);
            typeRect = GUILayoutUtility.GetLastRect();
            GUI.Label(typeRect, new GUIContent("", "The extension of the save files.\n\nYou can write any random file extension. Please don't use spaces or special characters. Avoid using pre-existing file extension and forbidden extension.\n\nYou can also use the \"Generate Random Extension\" button."));

            if (GUILayout.Button("Generate Random Extension"))
            {
                saveSystem.fileExtension = AASave.ExtensionRandomizer.GenerateRandomExtension(8);
            }

            GUILayout.Space(20F);

            GUILayout.Label("Encryption");
            saveSystem.encryptData = EditorGUILayout.Toggle("Encrypt the Data", saveSystem.encryptData);
            typeRect = GUILayoutUtility.GetLastRect();
            GUI.Label(typeRect, new GUIContent("", "If this is true, the game data will be encrypted before it is saved on the player's device."));

            if (saveSystem.encryptData)
            {
                if (GUILayout.Button("Randomize Encryption Key"))
                {
                    RandomizeEncryptionKey();
                }
                typeRect = GUILayoutUtility.GetLastRect();
                GUI.Label(typeRect, new GUIContent("", "Changes the current encryption key to a new, random value."));

                if (GUILayout.Button("Display Encryption Key"))
                {
                    DisplayEncryptionKey();
                }
                typeRect = GUILayoutUtility.GetLastRect();
                GUI.Label(typeRect, new GUIContent("", "Opens the default IDE to display encryption key."));

                if (_encryptionKeyRandomized)
                {
                    EditorGUILayout.HelpBox("Success! Encryption key has been randomized.", MessageType.Info);
                }

                EditorGUILayout.HelpBox("All the SaveSystem instances in your project uses the same encryption key.", MessageType.Info);
            }
            
            GUILayout.Space(10F);

            if (EditorGUI.EndChangeCheck())
            {
                if (target != null)
                {
                    Undo.RecordObject(target, "Changed Save System");
                }
            }
        }

        private void DisplayEncryptionKey()
        {
            string lPath = "AAEncryption.cs";
            foreach (var lAssetPath in AssetDatabase.GetAllAssetPaths())
            {
                if (lAssetPath.EndsWith(lPath))
                {
                    var target = (MonoScript)AssetDatabase.LoadAssetAtPath(lAssetPath, typeof(MonoScript));
                    if (target != null)
                    {
                        AssetDatabase.OpenAsset(target, 21);
                        break;
                    }
                }
            }
        }

        private void RandomizeEncryptionKey()
        {
            string newKey = AASave.EncryptionKeyRandomizer.GetRandomKey();
            string lPath = "AAEncryption.cs";

            foreach (var lAssetPath in AssetDatabase.GetAllAssetPaths())
            {
                if (lAssetPath.EndsWith(lPath))
                {
                    var target = (MonoScript)AssetDatabase.LoadAssetAtPath(lAssetPath, typeof(MonoScript));
                    if (target != null)
                    {
                        string scriptFilePath = AssetDatabase.GetAssetPath(target);
                        string[] lines = System.IO.File.ReadAllLines(scriptFilePath);

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].Contains("private static readonly string encryptionKey ="))
                            {
                                lines[i] = "        private static readonly string encryptionKey = \"" + newKey.ToString() + "\";";
                                System.IO.File.WriteAllLines(scriptFilePath, lines);
                                _encryptionKeyRandomized = true;
                                break;
                            }
                        }

                        AssetDatabase.Refresh();
                        AssetDatabase.SaveAssets();

                        break;
                    }
                }
            }
        }

    }
}

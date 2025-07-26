using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue SO/Base Dialogue")]

public class DialogueSO : ScriptableObject
{
    [Header("Standard Dialogue")]
    public string[] hitLines;
    public string[] proximityLines;

}

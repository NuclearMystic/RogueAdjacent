using UnityEngine;

[CreateAssetMenu]
public class SceneStaticData : ScriptableObject
{
    public Sprite sprite;
    public Vector2Int size = new Vector2Int(1, 1);
    public PlacementType placementType;
    public bool addOffset;
    public int health = 1;
    public bool nonDestructible;
    public AudioClip getHitSFX;
    public AudioClip destroySFX;
    public GameObject specificDropItem;
}

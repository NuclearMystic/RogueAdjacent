using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class SceneStatic : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D itemCollider;

    [SerializeField]
    private GameObject hitFeedback, destoyFeedback;

    public UnityEvent OnGetHit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Initialize(SceneStaticData sceneStaticData)
    {
        //set sprite
        spriteRenderer.sprite = sceneStaticData.sprite;
        //set sprite offset
        spriteRenderer.transform.localPosition = new Vector2(0.5f * sceneStaticData.size.x, 0.5f * sceneStaticData.size.y);
        itemCollider.size = sceneStaticData.size;
        itemCollider.offset = spriteRenderer.transform.localPosition;
    }

}


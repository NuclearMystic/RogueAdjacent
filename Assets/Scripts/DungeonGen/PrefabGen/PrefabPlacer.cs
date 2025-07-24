using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrefabPlacer : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    public List<GameObject> PlaceEnemies(List<EnemyPlacementData> enemyPlacementData,
                                         StaticPlacementHelper helper)
    {
        List<GameObject> placedObjects = new List<GameObject>();

        foreach (var placementData in enemyPlacementData)
        {
            for (int i = 0; i < placementData.Quantity; i++)
            {
                Vector2? spot = helper.GetItemPlacementPosition(
                    PlacementType.OpenSpace,
                    100,
                    placementData.enemySize,
                    false
                );

                if (spot.HasValue)
                {
                    placedObjects.Add(CreateObject(
                        placementData.enemyPrefab,
                        spot.Value + new Vector2(0.5f, 0.5f)));
                }
                else
                {
                    Debug.LogWarning($"Enemy placement failed for {placementData.enemyPrefab?.name} – no valid OpenSpace found.");
                }
            }
        }
        return placedObjects;
    }

    public List<GameObject> PlaceAllItems(List<ItemPlacementData> itemPlacementData,
                                          StaticPlacementHelper helper)
    {
        List<GameObject> placedObjects = new List<GameObject>();

        var sorted = itemPlacementData
            .OrderByDescending(d => d.sceneStaticData.size.x * d.sceneStaticData.size.y);

        foreach (var placementData in sorted)
        {
            for (int i = 0; i < placementData.Quantity; i++)
            {
                Vector2? spot = helper.GetItemPlacementPosition(
                    placementData.sceneStaticData.placementType,
                    100,
                    placementData.sceneStaticData.size,
                    placementData.sceneStaticData.addOffset
                );

                if (spot.HasValue)
                {
                    placedObjects.Add(PlaceItem(placementData.sceneStaticData, spot.Value));
                }
                else
                {
                    Debug.LogWarning(
                        $"Item placement failed for '{placementData.sceneStaticData.name}' " +
                        $"({placementData.sceneStaticData.placementType}) – no valid tile found.");
                }
            }
        }
        return placedObjects;
    }

    private GameObject PlaceItem(SceneStaticData item, Vector2 pos)
    {
        GameObject newItem = CreateObject(itemPrefab, pos);
        newItem.GetComponent<SceneStatic>().Initialize(item);
        return newItem;
    }

    public GameObject CreateObject(GameObject prefab, Vector3 pos)
    {
        if (prefab == null) return null;

        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        return go;
    }
}

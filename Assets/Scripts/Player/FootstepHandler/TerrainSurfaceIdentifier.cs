using UnityEngine;

public class TerrainSurfaceIdentifier : MonoBehaviour
{
    public Terrain terrain;
    public TerrainTextureSurface[] textureMappings;

    public SurfaceType GetDominantSurface(Vector3 worldPos)
    {
        if (terrain == null) return SurfaceType.Default;

        TerrainData data = terrain.terrainData;
        Vector3 terrainPos = worldPos - terrain.transform.position;

        int mapX = Mathf.Clamp((int)((terrainPos.x / data.size.x) * data.alphamapWidth), 0, data.alphamapWidth - 1);
        int mapZ = Mathf.Clamp((int)((terrainPos.z / data.size.z) * data.alphamapHeight), 0, data.alphamapHeight - 1);

        float[,,] alphas = data.GetAlphamaps(mapX, mapZ, 1, 1);
        int maxIndex = 0;
        float maxValue = 0f;

        for (int i = 0; i < data.alphamapLayers; i++)
        {
            if (alphas[0, 0, i] > maxValue)
            {
                maxIndex = i;
                maxValue = alphas[0, 0, i];
            }
        }

        foreach (var mapping in textureMappings)
        {
            if (mapping.textureIndex == maxIndex)
                return mapping.surfaceType;
        }

        return SurfaceType.Default;
    }
}

[System.Serializable]
public class TerrainTextureSurface
{
    public int textureIndex;
    public SurfaceType surfaceType;
}

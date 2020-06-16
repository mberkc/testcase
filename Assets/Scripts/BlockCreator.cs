using System.Collections.Generic;
using UnityEngine;

//In this class, the map has been created.
//You have to edit GetRelativeBlock section to calculate current relative block to cast player rope to hold on
//Update Block Position section to make infinite map.
public class BlockCreator : MonoBehaviour {

    private static BlockCreator singleton = null;
    private GameObject[] blockPrefabs;
    private GameObject pointPrefab;
    private GameObject pointObject;
    public int blockCount;

    private List<GameObject> blockPool = new List<GameObject> ();
    private float lastHeightUpperBlock = 10;
    private int difficulty = 1;

    int minZDiff = 3, totalBlocks, blocksToIncreaseDifficulty = 200;

    public static BlockCreator GetSingleton () {
        if (singleton == null) {
            singleton = new GameObject ("_BlockCreator").AddComponent<BlockCreator> ();
        }
        return singleton;
    }

    public void Initialize (int bCount, GameObject[] bPrefabs, GameObject pPrefab) {
        blockCount = bCount;
        blockPrefabs = bPrefabs;
        pointPrefab = pPrefab;
        InstantiateBlocks ();
    }

    public void InstantiateBlocks () {
        totalBlocks = 30;
        for (int i = 0; i < blockCount; i++) {
            lastHeightUpperBlock = Random.Range (lastHeightUpperBlock - difficulty, lastHeightUpperBlock + difficulty);
            float randomHeightLowerBlock = Random.Range (lastHeightUpperBlock - 20, lastHeightUpperBlock - 20 + difficulty * 3);
            GameObject newUpperBlock = Instantiate (blockPrefabs[i % blockPrefabs.Length], new Vector3 (0, lastHeightUpperBlock, i + 1), Quaternion.identity);
            GameObject newLowerBlock = Instantiate (blockPrefabs[i % blockPrefabs.Length], new Vector3 (0, randomHeightLowerBlock, i + 1), Quaternion.identity);
            blockPool.Add (newUpperBlock);
            blockPool.Add (newLowerBlock);
            if (i == 14) {
                pointObject = Instantiate (pointPrefab, new Vector3 (0, (lastHeightUpperBlock + randomHeightLowerBlock) / 2f, i + 1), Quaternion.Euler (90, 0, 0));
            }
        }
    }

    public int Difficulty {
        get {
            return difficulty;
        }

        set {
            difficulty = value;
        }
    }

    public Vector3 GetRelativeBlock (float playerPosZ) {
        int index = 0;
        Vector3 blockPos = blockPool[index].transform.position;
        if (blockPos.y < 0)
            blockPos = blockPool[++index].transform.position;
        float blockPosZ = blockPos.z;
        if (blockPosZ <= playerPosZ + minZDiff) {
            index += 2 * (Mathf.CeilToInt (playerPosZ - blockPosZ) + minZDiff);
            blockPos = blockPool[index].transform.position;
        }
        return blockPos;
    }

    public void UpdateBlockPosition (int blockIndex) {
        if (totalBlocks > blocksToIncreaseDifficulty) {
            blocksToIncreaseDifficulty *= 2;
            Difficulty++;
        }
        while (totalBlocks < blockIndex + 30) {
            lastHeightUpperBlock = Random.Range (lastHeightUpperBlock - difficulty, lastHeightUpperBlock + difficulty);
            float randomHeightLowerBlock = Random.Range (lastHeightUpperBlock - 20, lastHeightUpperBlock - 20 + difficulty * 3);
            GameObject newUpperBlock = blockPool[0];
            GameObject newLowerBlock = blockPool[1];
            blockPool.RemoveRange (0, 2);
            newUpperBlock.transform.position = new Vector3 (0, lastHeightUpperBlock, totalBlocks);
            newLowerBlock.transform.position = new Vector3 (0, randomHeightLowerBlock, totalBlocks);
            blockPool.Add (newUpperBlock);
            blockPool.Add (newLowerBlock);
            if (totalBlocks % 15 == 0)
                pointObject = Instantiate (pointPrefab, new Vector3 (0, (lastHeightUpperBlock + randomHeightLowerBlock) / 2f, totalBlocks), Quaternion.Euler (90, 0, 0));
            totalBlocks++;
        }
    }
}
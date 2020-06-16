//This class is on the main camera to follow player.
//You may optimize it on SetPosition section and
//Write a proper way to update blocks positions on the map to make it an infite gameplay.

using UnityEngine;

public class PlayerFollower : MonoBehaviour {

    public Transform player;
    float xPos, yDifference, zDifference;

    int lastPassageIndex = -1, passageConstant = 10;

    void Awake () {
        xPos = transform.position.x;
        yDifference = player.position.y - transform.position.y;
        zDifference = player.position.z - transform.position.z;
    }
    private void LateUpdate () {
        if (player != null) {
            transform.position = new Vector3 (xPos, player.position.y - yDifference, player.position.z - zDifference);

            int passageIndex = Mathf.CeilToInt (player.position.z);
            if (passageIndex > lastPassageIndex + passageConstant) {
                lastPassageIndex = passageIndex;
                BlockCreator.GetSingleton ().UpdateBlockPosition (lastPassageIndex);
            }
        }
    }

}
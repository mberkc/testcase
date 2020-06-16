using System.Collections;
using UnityEngine;

//In this section, you have to edit OnPointerDown and OnPointerUp sections to make the game behave in a proper way using hJoint
//Hint: You may want to Destroy and recreate the hinge Joint on the object. For a beautiful gameplay experience, joint would created after a little while (0.2 seconds f.e.) to create mechanical challege for the player
//And also create fixed update to make score calculated real time properly.
//Update FindRelativePosForHingeJoint to calculate the position for you rope to connect dynamically
//You may add up new functions into this class to make it look more understandable and cosmetically great.

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private GameObject[] blockPrefabs;
    [SerializeField]
    private HingeJoint hJoint;
    [SerializeField]
    private LineRenderer lRenderer;
    [SerializeField]
    private Rigidbody playerRigidbody;
    [SerializeField]
    private PlayerFollower playerFollower;
    [SerializeField]
    private GameObject pointPrefab;
    [SerializeField]

    private GUIController guiController;

    private float score;

    private bool gameOver = false;

    Coroutine createJointCoroutine;

    bool isPointerDown, firstTouch = true;

    void Start () {
        BlockCreator.GetSingleton ().Initialize (30, blockPrefabs, pointPrefab);
        FindRelativePosForHingeJoint (new Vector3 (0, 10, 0));
    }

    public void FindRelativePosForHingeJoint (Vector3 blockPosition) {
        hJoint.anchor = transform.InverseTransformDirection (blockPosition - transform.position);
        lRenderer.SetPosition (1, hJoint.anchor);
        lRenderer.enabled = true;
    }

    public void PointerDown () {
        isPointerDown = true;
        if (!firstTouch)
            createJointCoroutine = StartCoroutine ("CreateHingeJoint");
        else {
            firstTouch = false;
            guiController.CloseHoldStartText ();
        }
    }

    public void PointerUp () {
        if (createJointCoroutine != null)
            StopCoroutine (createJointCoroutine);
        isPointerDown = false;
        DestroyHingeJoint ();
    }

    IEnumerator CreateHingeJoint () {
        yield return new WaitForSeconds (0.2f);
        hJoint = gameObject.AddComponent<HingeJoint> ();
        Vector3 blockTransform = BlockCreator.GetSingleton ().GetRelativeBlock (transform.position.z);
        FindRelativePosForHingeJoint (blockTransform);
    }

    void DestroyHingeJoint () {
        hJoint = null;
        Destroy (GetComponent<HingeJoint> ());
        lRenderer.enabled = false;
    }

    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag ("Block") && !gameOver) {
            PointerUp (); //Finishes the game here to stoping holding behaviour
            gameOver = true;
            guiController.CloseRealtimeScoreText ();
            guiController.SetScoreText (score);
            if (PlayerPrefs.HasKey ("HighScore")) {
                float highestScore = PlayerPrefs.GetFloat ("HighScore");
                if (score > highestScore)
                    highestScore = score;
                SetHighScore (highestScore);
            } else
                SetHighScore (score);
            guiController.OpenGameOverPanel ();
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag ("Point")) {
            if (Vector3.Distance (transform.position, other.gameObject.transform.position) < .5f)
                score += 10f;
            else
                score += 5f;
            other.gameObject.SetActive (false);
        }
    }
    private void FixedUpdate () {
        SetScore ();
        if (isPointerDown)
            playerRigidbody.AddForce (Vector3.forward / 20, ForceMode.Impulse); // Vector3.forward / 5, ForceMode.VelocityChange - Faster start with ignoring mass
    }

    void SetScore () {
        // Time.deltaTime
        score += playerRigidbody.velocity.z * Time.fixedDeltaTime * 0.1f;
        guiController.SetRealtimeScoreText (score);
    }

    void SetHighScore (float score) {
        PlayerPrefs.SetFloat ("HighScore", score);
        guiController.SetHighScoreText (score);
    }
}
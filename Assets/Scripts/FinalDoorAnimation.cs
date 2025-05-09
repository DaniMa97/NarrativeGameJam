using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class FinalDoorAnimation : MonoBehaviour
{
    GameObject player;
    public GameObject monster;
    public float interactRange = 3f;
    public float monsterOffset = 2;

    AudioSource jumpScare;
    public AudioClip jumpScareClip;

    float playerYRot;
    float error = 0.01f;

    float currentRot = 0f;

    private void Start()
    {
        player = GameObject.Find("Player");
        jumpScare = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.transform.position, transform.position) <= interactRange)
        {
            player.GetComponent<PlayerController>().PlayerWon();
            monster.GetComponent<NavMeshAgent>().enabled = false;
            monster.GetComponent<EnemyController>().Stop();
            Vector3 monsterPos = player.transform.position;
            monsterPos.x += monsterOffset;
            playerYRot = player.transform.rotation.eulerAngles.y;
            monster.transform.position = monsterPos;
            monster.transform.rotation = Quaternion.Euler(0, -90, 0);
            StartCoroutine(RotatePlayer());
        }
    }

    IEnumerator RotatePlayer()
    {
        while(player.transform.rotation.eulerAngles.y - 90 > error)
        {
            Vector3 playerRot = player.transform.rotation.eulerAngles;
            playerRot.y = Mathf.Lerp(playerYRot, 90, currentRot);
            player.transform.rotation = Quaternion.Euler(playerRot);
            currentRot += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        jumpScare.PlayOneShot(jumpScareClip, SoundController.GetInstance().GetSfxVolume());
        yield return new WaitForSeconds(1f);
        GameObject.FindFirstObjectByType<MainGameEvents>().PlayerWin();
    }
}

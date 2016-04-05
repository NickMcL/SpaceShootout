using UnityEngine;
using System.Collections;

public class TacklePowerUp : MonoBehaviour {

    public float TackleModifier = 2f;
    public float time = 10f;
    bool canBePickedUp = true;
    bool followingTheLeader = false;

    Player p;
    Transform following;

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && canBePickedUp) {
            HUD.S.PlaySound("powerup");
            GetComponent<SpriteRenderer>().enabled = false;
            HUD.S.GetPushPowerup();
            canBePickedUp = false;
            p = collision.GetComponent<Player>();
            following = collision.gameObject.transform;
            followingTheLeader = true;
            BuffPlayer();
            Invoke("NerfPlayer", time);
        }
    }

    void BuffPlayer() {
        p.pushPoweruped = true;

        p.pushSpeed *= TackleModifier;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void NerfPlayer() {
        p.pushPoweruped = false;
        p.pushSpeed /= TackleModifier;
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().enableEmission = false;
        StartCoroutine(DieAfterALil());
    }

    IEnumerator DieAfterALil() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }



    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (followingTheLeader) {
            transform.position = following.position;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}

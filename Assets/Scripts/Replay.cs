using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Replay : MonoBehaviour {
    List<replay_object> moving_objects;

    public int current_time = 0;
    int replay_time = 0;
    int replay_stop = 0;
    int total_objects = 0;
    int objects_updated = 0;
    bool playing_back = false;
    public static Replay replay_device;

    public struct replay_object {
        public GameObject game_object;
        public int start_time;
        public int end_time;
        public Vector3[] positions;
        public Quaternion[] rotations;
        public GameObject ore_no_kamen; //visual clone of game_object
        public GameObject playback_object;
    }
    // Use this for initialization
    void Awake() {
        replay_device = this;
        moving_objects = new List<replay_object>();
    }
    void Start() {

    }
    //must be called on start by all objects to be replayed
    public void addToList(GameObject new_object) {
        replay_object new_moving_object = new replay_object();

        new_moving_object.game_object = new_object;
        new_moving_object.start_time = current_time;
        new_moving_object.end_time = 9999;
        new_moving_object.positions = new Vector3[2700];
        new_moving_object.rotations = new Quaternion[2700];
        moving_objects.Add(new_moving_object);
        total_objects++;
    }
    //must be called in FixedUpdate of all objects to be replayed
    public void updateObject(GameObject known_object) {
        objects_updated++;
        int index = moving_objects.FindIndex(x => x.game_object.name == known_object.name);
        moving_objects[index].positions[current_time] = known_object.transform.position;
        moving_objects[index].rotations[current_time] = known_object.transform.rotation;
        checkNextFrame();
    }
    public void killObject(GameObject known_object) {
        objects_updated--;
        total_objects--;
        int index = moving_objects.FindIndex(x => x.game_object.name == known_object.name);
        replay_object dupe = moving_objects[index];
        dupe.end_time = current_time - 1;
        moving_objects[index] = dupe;
        //checkNextFrame();
    }
    void checkNextFrame() {
        if (total_objects != objects_updated)
            return;
        else {
            current_time++;
            objects_updated = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate() {
        if (playing_back)
            playBack();
    }

    public void startPlayback(int replay_start_time, int replay_stop_time,float time_scale) {
        Time.timeScale = time_scale;
        if (replay_stop_time <= replay_start_time || replay_stop_time > current_time) {
            print("invalid replay, see startPlayback");
            return;
        }
        replay_time = replay_start_time;
        replay_stop = replay_stop_time;
        playing_back = true;
        for (int i = 0; i < moving_objects.Count; i++) {


            if (replay_time >= moving_objects[i].start_time) {
                MonoBehaviour[] scripts = moving_objects[i].game_object.GetComponents<MonoBehaviour>();
                Collider2D coll = moving_objects[i].game_object.GetComponent<Collider2D>();
                coll.enabled = false;
                foreach (MonoBehaviour script in scripts) {
                    script.enabled=false;
                }
                replay_object dupe = moving_objects[i];
                dupe.playback_object = Instantiate(moving_objects[i].game_object);

                foreach (MonoBehaviour script in scripts) {
                    script.enabled = true;
                }
                coll.enabled = true;
                dupe.game_object.SetActive(false);
                moving_objects[i] = dupe; //it has to be

            }
        }
    }
    void playBack() {
        if (replay_time == replay_stop) {
            stopPlayBack();
            return;
        }
        for (int i = 0; i < moving_objects.Count; i++) {

            if (replay_time == moving_objects[i].start_time) {

                replay_object dupe = moving_objects[i];
                dupe.playback_object = Instantiate(moving_objects[i].game_object);
                moving_objects[i] = dupe; //it has to be
            }
            if (replay_time > moving_objects[i].start_time && replay_time < moving_objects[i].end_time) {
                moving_objects[i].playback_object.transform.position = moving_objects[i].positions[replay_time];
                moving_objects[i].playback_object.transform.rotation = moving_objects[i].rotations[replay_time];
            }
            if (replay_time == moving_objects[i].end_time) {
                Destroy(moving_objects[i].playback_object);
            }
        }
        replay_time++;
    }
    void stopPlayBack() {
        playing_back = false;
        for (int i = 0; i < moving_objects.Count; i++) {


            if (replay_time >= moving_objects[i].start_time && replay_time <= moving_objects[i].end_time) {
                Destroy(moving_objects[i].playback_object);
                moving_objects[i].game_object.SetActive(true);


            }
        }
        Time.timeScale = 1;

    }
}

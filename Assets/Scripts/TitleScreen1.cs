using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen1 : MonoBehaviour
{
    public AudioSource audio;
    public Image flash;
    public GameObject title1;
    public GameObject button;
    public GameObject background;
    public AudioClip whoosh1, whoosh2;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(animatewoosh());
        audio.Pause();
    }
    IEnumerator animatewoosh()
    {
        AudioSource.PlayClipAtPoint(whoosh1, Camera.main.transform.position, 1f);
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(whoosh2, Camera.main.transform.position, 1f);
    }
    bool title2move = false;
    IEnumerator animate()
    {
        background.SetActive(true);
        button.SetActive(true);
        audio.UnPause();
        Color c = flash.color;
        c.a = 1f;
        flash.color = c;
        for (int cd = 0; cd < 5; ++cd)
        {
            yield return new WaitForSeconds(0.05f);
            c.a -= 0.25f;
            flash.color = c;
        }
        c.a = 0f;
        flash.color = c;
        yield return new WaitForSeconds(2f);
    }
    public Vector3 title1dest, title2dest;
    bool doinit = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if ((title1.transform.localPosition - title1dest).magnitude > 5)
        {
            title1.transform.localPosition = Vector3.Lerp(title1.transform.localPosition, title1dest, Time.deltaTime * 5f);
        }
        else
        {
            title2move = true;
        }
            if (title2move && !doinit)
            {
                doinit = true;
                StartCoroutine(animate());
            }
    }

    public void StartGame(int scene)
    {
        Application.LoadLevel(scene);
    }
    public string destination = "Juice";
    void Update()
    {
        if (ControlManager.playerPressedStart())
        {
            SceneManager.LoadScene(destination);
        }
    }
}

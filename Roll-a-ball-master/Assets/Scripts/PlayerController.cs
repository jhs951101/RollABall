using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GUIText countText;
    public GUIText winText;
    public Slider timeBar;  // timeBar: 게임 화면 상단 우측의 Slider
    public GameObject randomCube;  // randomCube: 2초마다 임의로 움직이는 Cube
    private int count;
    private int numberOfGameObjects;
    private float elapseTime;  // elapseTime: 시간 경과를 의미
    private float moveTime;  // moveTime: Cube 하나가 임의로 움직이도록 하기 위해 사용

    void Start()
    {
        count = 0;
        numberOfGameObjects = GameObject.FindGameObjectsWithTag("PickUp").Length;
        SetCountText();
        winText.text = "";
        elapseTime = 0;  // 초기 시간 경과는 0으로 세팅함
        timeBar = GameObject.Find("Timebar").GetComponent<Slider>();  // 게임 화면의 Slider를 연결시킴
        randomCube = GameObject.Find("RandomPickUp");  // 게임 화면의 Cube를 연결시킴
    }

    void Update()
    {
        elapseTime += Time.deltaTime;
        timeBar.value = elapseTime;  // Slider의 값을 변경함

        moveTime += Time.deltaTime;

        if ((int)moveTime == 2)  // moveTime이 0 -> 1 -> 2 가 되는 순간에 Cube 하나의 위치가 임의로 바뀜
        {
            randomCube.transform.position = new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f));
            moveTime = 0;
        }

        if ((int)elapseTime >= 180)  // 시간 초과 시 텍스트 출력 후 게임 종료
        {
            winText.text = "TIME OVER...";
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= numberOfGameObjects)  // Cube 5개를 모두 먹었을 경우 텍스트 출력 후 게임 종료
        {
            winText.text = "YOU WIN!";
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}

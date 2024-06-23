using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{

    public GameObject createJoinRoombg;
    public GameObject joinRoombtn;
    public GameObject createRoombtn;
    //input field
    public TMP_InputField INname,INroom_name;

    // Start is called before the first frame update
    void Start()
    {
        joinRoombtn.SetActive(false);
        createRoombtn.SetActive(false);
        createJoinRoombg.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {

        PlayerPrefs.SetString("name", INname.text);
        PlayerPrefs.SetString("type", "HOST");
        PlayerPrefs.SetString("room_name", INroom_name.text);
        SceneManager.LoadScene("GamePlay");
    }


    public void JoinRoom()
    {

        PlayerPrefs.SetString("name", INname.text);
        PlayerPrefs.SetString("type", "CLIENT");
        PlayerPrefs.SetString("room_name", INroom_name.text);
        SceneManager.LoadScene("GamePlay");
    }
}

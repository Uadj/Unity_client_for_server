using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;

public class TextLeaderboard : MonoBehaviour
{
    private string sendUrl = "localhost:8080/upload";
    private string getUrl = "localhost:8080/download";

    [SerializeField]
    private GameObject rankingPanel;
    [SerializeField]
    private GameObject userData;
    [SerializeField]
    private Transform parent;

    private Vector3 textSpawnPosition;
    private float posIncrement = -50f;

    [System.Serializable]
    public class UserData
    {
        public string username;
        public int score;
    }

    private void Start()
    {
        textSpawnPosition = parent.transform.position;
    }
    public void CallData()
    {
        rankingPanel.SetActive(true);
        StartCoroutine(GetData());
    }
    public void SendData(string username, int score)
    {
        string StringScore = score.ToString();
        string json = "{\"username\":\"" + username + "\",\"score\":" + score + "}";
        StartCoroutine(SendJSONData(json));
    }   
    private IEnumerator SendJSONData(string json)
    {
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["Content-Type"] = "application/json";

        WWW www = new WWW(sendUrl, postData, headers);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Error sending data: " + www.error);
        }
        else
        {
            Debug.Log("Data sent successfully.");
        }
    }
    private IEnumerator GetData()
    {
        UnityWebRequestAsyncOperation operation = UnityWebRequest.Get(getUrl).SendWebRequest();
        yield return operation;

        if (operation.webRequest.isNetworkError || operation.webRequest.isHttpError)
        {
            Debug.Log(operation.webRequest.error);
        }
        else
        {
            var data = operation.webRequest.downloadHandler.text;
            var players = JsonConvert.DeserializeObject<UserData[]>(data);
            
            foreach (var player in players)
            {
                GameObject playerData = Instantiate(userData, textSpawnPosition, Quaternion.identity, parent);
                TMP_Text usernameText = playerData.transform.Find("Username").GetComponent<TMP_Text>();
                TMP_Text scoreText = playerData.transform.Find("Score").GetComponent<TMP_Text>();
                textSpawnPosition.y += posIncrement;
                usernameText.text = player.username;
                scoreText.text = player.score.ToString();
                Debug.Log(player.username);
            }
        }
    }




}

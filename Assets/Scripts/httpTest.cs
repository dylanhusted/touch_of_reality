using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

public class httpTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(GetNotifications());
    }

    IEnumerator GetNotifications()
    {
        // Get TextMesh
        TextMesh pointsText = GameObject.Find("apiResponse").GetComponent<TextMesh>();
        pointsText.text = "Ready.";
        // Call API
        WWWForm form = new WWWForm();
        var headers = new Dictionary<string, string>();
        headers.Add("Access-Token", "o.czs8GXB8DKYttsEtWsBb991gQdHSsGSE");
        WWW www = new WWW("https://api.pushbullet.com/v2/pushes", null, headers);
        yield return www;
        Debug.Log("2. " + www.text);
        // Parse JSON
        Response notif = JsonUtility.FromJson<Response>(www.text);
        // Iterate over each Push
        for (int i = 0; i < notif.pushes.Count; i++)
        {
            // Display notification, based on push type
            if (notif.pushes[i].type == "note")
            {
                Debug.Log(notif.pushes[i].sender_name + ": " + notif.pushes[i].body);
            }
            else if (notif.pushes[i].type == "file")
            {
                Debug.Log(notif.pushes[i].sender_name + " sent you a file");
            }
            else
            {
                Debug.Log(notif.pushes[i].sender_name + ": [TORE-error] Unknown notification type");
            }
        }
        // Set TextMesh
        pointsText.text = www.text;
    }

    [System.Serializable]
    public class Response
    {
        public List<Push> pushes;
    }

    [System.Serializable]
    public class Push
    {
        public string type;
        public string sender_name;
        public string body;
    }
}

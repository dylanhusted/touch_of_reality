using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class wsTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(StreamNotifications());
    }

    IEnumerator StreamNotifications()
    {
        WebSocket w = new WebSocket(new Uri("wss://stream.pushbullet.com/websocket/o.czs8GXB8DKYttsEtWsBb991gQdHSsGSE"));
        yield return StartCoroutine(w.Connect());
        while (true)
        {
            string reply = w.RecvString();
            if (reply != null)
            {
                // Debug.Log("Received: " + reply);
                var resp = JSON.Parse(reply);
                if (resp["type"] == "push")
                {
                    if (resp["push"]["type"] == "sms_changed")
                    {
                        Debug.Log(resp["push"]["notification"]["title"] + ": " + resp["push"]["notification"]["body"]);
                    }
                    //if (resp["push"]["type"] == "mirror")
                    //{
                    //    if (resp["push"]["notification"]["application_name"] == "Messenger")
                    //    {
                    //        Debug.Log(resp["push"]["notification"]["title"] + ": " + resp["push"]["notification"]["body"]);
                    //    }
                    //}
                }
            }
            if (w.error != null)
            {
                Debug.LogError("Error: " + w.error);
                break;
            }
            yield return 0;
        }
        w.Close();
    }
}

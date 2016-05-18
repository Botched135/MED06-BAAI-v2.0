using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using XInputDotNetPure;

public class UDPClient : MonoBehaviour{

    const int CONFIG_PORT = 9876;
    const string CONFIG_IP = "127.0.0.1";
    Thread receiveThread;
    UdpClient client;
    private Rumble rumble;
    private GameAI AIController;
    [SerializeField]
    [Range(40,100)]
    private int GSRDifference; 

    Vector3 position;

	// Use this for initialization
	void Start () {
        rumble = GetComponent<Rumble>();
        AIController = GetComponent<GameAI>();
        init();
	}
	
	// Update is called once per frame
	void Update () {
        if(rumble == null)
        {
            rumble = GetComponent<Rumble>();
        
        }
	}

    private void init()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));

        receiveThread.IsBackground = true;
        receiveThread.Start();

    }
    private void ReceiveData()
    {
        client = new UdpClient(CONFIG_PORT);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 9876);
        bool done = false;

        string received_data;
        byte[] receive_byte_array;
        try
        {
            while (!done)
            {
                receive_byte_array = client.Receive(ref groupEP);
                received_data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);


                if (received_data != null)
                {

                    
                    float time = 0, BPM = 0;
                    int HRD = 0, GSR = 0, BaseLine = 0;
                 
                    int firstSpaceIndex = received_data.IndexOf(",", 0);
                    int secondSpaceIndex = received_data.IndexOf(",", firstSpaceIndex+1);
                    int thirdSpaceIndex = received_data.IndexOf(",", secondSpaceIndex + 1);
                    int fouthSpaceIndex = received_data.IndexOf(",", thirdSpaceIndex + 1);
                    int fifthSpaceIndex = received_data.IndexOf(",", fouthSpaceIndex + 1);
                    int sixthSpaceIndex = received_data.IndexOf(",", fifthSpaceIndex + 1);
 
                    time = float.Parse(received_data.Substring(secondSpaceIndex+1, thirdSpaceIndex - secondSpaceIndex));
                    GSR = int.Parse(received_data.Substring(thirdSpaceIndex + 1, fouthSpaceIndex - thirdSpaceIndex-1));
                    BaseLine = int.Parse(received_data.Substring(fouthSpaceIndex + 1, fifthSpaceIndex - fouthSpaceIndex-1));
                    BPM = float.Parse(received_data.Substring(fifthSpaceIndex + 1,sixthSpaceIndex-fifthSpaceIndex-1));
                    HRD = int.Parse(received_data.Substring(sixthSpaceIndex + 1));


                    AIController.time = time;
                    if (HRD != 0)
                    {
                        AIController.HRV.Add(HRD);
                        AIController.BPM.Add(BPM);
                        rumble.Shake(Mathf.Abs(GSR-BaseLine));
                    }
                    if (Mathf.Abs(GSR - BaseLine) >= GSRDifference)
                    {
                        AIController.GSRSpikes++;
                    }
                    AIController.GSR.Add(GSR);               

                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        client.Close();
    }

    public void OnApplicationQuit()
    {
        if (receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }

        client.Close();
    }
}

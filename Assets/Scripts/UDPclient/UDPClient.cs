using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

public class UDPClient : MonoBehaviour{

    const int CONFIG_PORT = 9876;
    const string CONFIG_IP = "127.0.0.1";
    Thread receiveThread;
    UdpClient client;

    Vector3 position;

	// Use this for initialization
	void Start () {

        init();
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void init()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));

        receiveThread.IsBackground = true;
        receiveThread.Start();

        Debug.Log("Start");
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

                    Debug.Log(received_data);   
                    float GSR=0, BPM=0, BaseLine= 0, Difference=0;
                    string tryDiffernce;
                    int firstSpaceIndex = received_data.IndexOf(" ", 0);
                    int secondSpaceIndex = received_data.IndexOf(" ", firstSpaceIndex+1);
                    int thirdSpaceIndex = received_data.IndexOf(" ", secondSpaceIndex + 1);

                    GSR = float.Parse(received_data.Substring(0, firstSpaceIndex));
                    BaseLine = float.Parse(received_data.Substring(firstSpaceIndex + 1, secondSpaceIndex - firstSpaceIndex));
                    BPM = float.Parse(received_data.Substring(secondSpaceIndex + 1, thirdSpaceIndex - secondSpaceIndex));
                    tryDiffernce = received_data.Substring(thirdSpaceIndex + 1);
                   /* if (!tryDiffernce.Equals("WaitForHeartBeat"))
                        Difference = float.Parse(tryDiffernce);*/
                    DataHolder.BPM = BPM;
                    DataHolder.Baseline = BaseLine;
                    DataHolder.GSR = GSR;
                   // DataHolder.Difference = Difference;
                    
                

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

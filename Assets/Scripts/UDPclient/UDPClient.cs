using UnityEngine;
using System;
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

    Vector3 position;

	// Use this for initialization
	void Start () {
        rumble = GetComponent<Rumble>();
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

                     
                    float time = 0, GSR=0, BPM=0, BaseLine= 0, HRD=0;
                 
                    int firstSpaceIndex = received_data.IndexOf(" ", 0);
                    int secondSpaceIndex = received_data.IndexOf(" ", firstSpaceIndex+1);
                    int thirdSpaceIndex = received_data.IndexOf(" ", secondSpaceIndex + 1);
                    int fouthSpaceIndex = received_data.IndexOf(" ", thirdSpaceIndex + 1);

                    time = float.Parse(received_data.Substring(0, firstSpaceIndex));
                    GSR = float.Parse(received_data.Substring(firstSpaceIndex + 1, secondSpaceIndex - firstSpaceIndex));
                    BaseLine = float.Parse(received_data.Substring(secondSpaceIndex + 1, thirdSpaceIndex - secondSpaceIndex));
                    BPM = float.Parse(received_data.Substring(thirdSpaceIndex + 1,fouthSpaceIndex-thirdSpaceIndex));
                    HRD = float.Parse(received_data.Substring(fouthSpaceIndex + 1));

                    if (HRD > 0)
                    {
                        rumble.Shake(Mathf.Abs(GSR-BaseLine));
                    }               

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

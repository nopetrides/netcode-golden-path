using Unity.Netcode;
using UnityEngine;

public class HelloWorldNetcodeManager : MonoBehaviour
{
    private const string Host = "Host";
    private const string Client = "Client";
    private const string Server = "Server";
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10,10,300,300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            SubmitNewPosition();
        }
        
        GUILayout.EndArea();
    }
    
    private void StartButtons()
    {
        if (GUILayout.Button(Host)) 
            NetworkManager.Singleton.StartHost();
        if (GUILayout.Button(Client))
            NetworkManager.Singleton.StartClient();
        if (GUILayout.Button(Server))
            NetworkManager.Singleton.StartServer();
    }

    private void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ? Host : NetworkManager.Singleton.IsServer ? Server : Client;
        
        GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
    
    private void SubmitNewPosition()
    {
        if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
        {
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            {
                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldNetcodePlayer>()
                        .Move();
            }
            else
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldNetcodePlayer>();
                player.Move();
            }
        }
    }
}

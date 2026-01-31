using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public static class RelayManager {

    public static string LobbyCode { get; private set; }
    
    // Allocate a new relay server with a maximum number of connections.
    public static async Task<bool> CreateRelay(int maxConnections) {

        // Initialize all Unity Services subscribed to Core, and sign in anonymously to use Relay.
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        try {
            // Create a new relay allocation with a maximum number of connections.
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            LobbyCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            // Set the relay server data for the network manager and start the host.
            // Connection type can be "dtls", "udp" or "wss".
            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "wss");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        } catch (RelayServiceException e) {
            Debug.LogError("Failed to create relay: " + e.Message);
            return false;
        }

        return true;
    }

    // Join a relay server using a join code.
    public static async Task<bool> JoinRelay(string joinCode) {
        if (joinCode == null || joinCode == "") {
            Debug.LogError("Join code is null or empty.");
            return false;
        }

        // If the network manager is already running, shut it down before joining a new relay.
        NetworkManager.Singleton.Shutdown();

        // Initialize all Unity Services subscribed to Core, and sign in anonymously to use Relay.
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        try {

            // Get the allocation from the join code and set the relay server data for the network manager.
            // Connection type can be "dtls", "udp" or "wss".
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "wss");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e) {
            Debug.LogError("Failed to join relay: " + e.Message);
            return false;
        }

        return true;
    }

    // Disconnect from the relay server.
    public static void Disconnect() {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer) {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("Disconnected from relay.");
        } else {
            Debug.LogWarning("Cannot disconnect: Not connected to any relay.");
        }
    }
}

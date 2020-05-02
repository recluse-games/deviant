using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Deviant;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityAsync;

public class EncounterState : MonoBehaviour
{
    public Deviant.Encounter encounter = default;
    private Deviant.EncounterService.EncounterServiceClient _client;
    private Channel _channel;
    private string _server = "127.0.0.1:50051";

    private void DeviantClient()
    {
        this._channel = new Channel(_server, ChannelCredentials.Insecure);
        this._client = new Deviant.EncounterService.EncounterServiceClient(_channel);
    }

    // Start is called before the first frame update
    async void Start()
    {
        DeviantClient();
        await CreateEncounterAsync();
    }

    void Update()
    {
        Debug.Log(encounter);
    }

    public Encounter GetEncounter()
    {
        return this.encounter;
    }
    public async Task<bool> CreateEncounterAsync()
    {
        try
        {
            using (var call = _client.StartEncounter())
            {
                Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
                encounterRequest.PlayerId = "0000";

                await call.RequestStream.WriteAsync(encounterRequest);
                var responseReaderTask = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        Encounter encounterResponseData = call.ResponseStream.Current.Encounter;
                        this.encounter = encounterResponseData;
                    }
                });

                await call.RequestStream.CompleteAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            throw ex;
        }
    }

    public async Task<bool> UpdateEncounterAsync(Deviant.EncounterRequest encounterRequest)
    {
        try
        {
            using (var call = _client.UpdateEncounter())
            {
                await call.RequestStream.WriteAsync(encounterRequest);
                var responseReaderTask = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        Encounter encounterResponseData = call.ResponseStream.Current.Encounter;
                        this.encounter = encounterResponseData;
                    }
                });

                await call.RequestStream.CompleteAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            throw ex;
        }
    }
}

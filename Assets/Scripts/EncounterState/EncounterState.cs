using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Deviant;
using UnityEngine;
using UnityAsync;
using UnityEngine.PlayerLoop;

public class EncounterState : MonoBehaviour
{
    public Deviant.Encounter encounter = default;
    private Deviant.EncounterService.EncounterServiceClient _client;
    private Channel _channel;
    private string _player = default;
    private string _server = default;

    private void DeviantClient()
    {
        this._channel = new Channel(this._server, ChannelCredentials.Insecure);
        this._client = new Deviant.EncounterService.EncounterServiceClient(_channel);
    }
    // Start is called before the first frame update
    async void Start()
    {
        this._server = PlayerPrefs.GetString("server");
        this._player = PlayerPrefs.GetString("playerId");

        DeviantClient();
        await CreateEncounterAsync();
    }

    public string GetPlayerId()
    {
        return this._player;
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
                encounterRequest.PlayerId = this._player;

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


    async void Update()
    {
        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.PlayerId = _player;
        encounterRequest.Encounter = encounter;
        encounterRequest.GetEncounterState = true;
 
        await UpdateEncounterAsync(encounterRequest);
    }
    public void OnDestroyed()
    {
        _channel.ShutdownAsync().Wait();
    }
}

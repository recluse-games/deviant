using System;
using System.Threading.Tasks;
using Grpc.Core;
using Deviant;
using UnityEngine;
using ObserverPattern;
using UnityAsync;

public class EncounterState : MonoBehaviour
{
    public Deviant.Encounter encounter = default;

    private Deviant.EncounterService.EncounterServiceClient _client;
    private Channel _channel;
    private string _player = default;
    private string _server = default;

    //Will send notifications that something has happened to whoever is interested
    public GameObject subject = default;

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
        SetupSubject();

        await GetUpdatedEncounter();
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


    public void SetupSubject()
    {
        GameObject newSubjectGameObject = new GameObject();
        newSubjectGameObject.AddComponent<Subject>();
        this.subject = newSubjectGameObject;
    }

    public void AddEntityObserver(GameObject entityObj)
    {
        GameObject newGameObject = new GameObject();

        newGameObject.AddComponent<EntityObserver>();
        newGameObject.GetComponent<EntityObserver>().SetEntity(entityObj);
        newGameObject.GetComponent<EntityObserver>().SetEncounterEvents(new GetEncounter());

        subject.GetComponent<Subject>().AddObserver(newGameObject);
    }

    public async Task<bool> CreateEncounterAsync()
    {
        await Await.BackgroundSyncContext();

        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.PlayerId = this._player;

        try
        {
            var call = _client.StartEncounter();
            await call.RequestStream.WriteAsync(encounterRequest);

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            throw ex;
        }
    }

    public async Task<bool> UpdateEncounterAsync(Deviant.EncounterRequest encounterRequest)
    {
        await Await.BackgroundSyncContext();

        try
        {
            var call = _client.UpdateEncounter();
            await call.RequestStream.WriteAsync(encounterRequest);

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            throw ex;
        }
    }

    public async Task<bool> ProcessStreamResponse(AsyncDuplexStreamingCall<EncounterRequest, EncounterResponse> call)
    {
        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.EntityGetAction = new Deviant.EntityGetAction();
        encounterRequest.PlayerId = this._player;

        await call.RequestStream.WriteAsync(encounterRequest);

        while (true)
        {
            if (await call.ResponseStream.MoveNext())
            {
                Encounter encounterResponseData = call.ResponseStream.Current.Encounter;
                encounter = encounterResponseData;
                Debug.Log(encounter);
            }
            else
            {
                Debug.Log("empty");
                continue;
            }
        }
    }

    public async Task<bool> GetUpdatedEncounter()
    {
        try
        {
            var call = _client.UpdateEncounter();

            await ProcessStreamResponse(call);

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            throw ex;
        }
    }

    public void Update()
    {
        this.subject.GetComponent<Subject>().Notify(encounter);
    }

    public void OnDestroyed()
    {
        _channel.ShutdownAsync().Wait();
    }
}

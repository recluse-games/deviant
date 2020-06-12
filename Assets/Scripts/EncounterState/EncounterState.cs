using System;
using System.Threading;
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
    private AsyncDuplexStreamingCall<EncounterRequest, EncounterResponse> _call = default;
    private SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);
    private SemaphoreSlim _readLock = new SemaphoreSlim(1, 1);

    //Will send notifications that something has happened to whoever is interested
    public GameObject subject = default;

    private void DeviantClient()
    {
        this._channel = new Channel(this._server, ChannelCredentials.Insecure);
        this._client = new Deviant.EncounterService.EncounterServiceClient(_channel);
        this._call = _client.UpdateEncounter();
    }

    // Start is called before the first frame update
    async void Start()
    {
        this._server = PlayerPrefs.GetString("server");
        this._player = PlayerPrefs.GetString("playerId");
        
        DeviantClient();
        SetupSubject();

        await CreateEncounterAsync();
        await GetUpdatedEncounter();
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
        GameObject newEntityObserver = new GameObject();
        newEntityObserver.transform.name = "observer_entity_" + entityObj.GetComponentInChildren<Entity>().GetId();
        newEntityObserver.AddComponent<EntityObserver>();
        newEntityObserver.GetComponent<EntityObserver>().SetEntity(entityObj);
        newEntityObserver.GetComponent<EntityObserver>().SetEncounterEvents(new GetEncounter());

        subject.GetComponent<Subject>().AddObserver(newEntityObserver);
    }

    public void RemoveEntityObserver(GameObject entityObj)
    {
        subject.GetComponent<Subject>().RemoveObserver(entityObj);
    }

    public async Task<bool> CreateEncounterAsync()
    {
        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.EncounterCreateAction = new Deviant.EncounterCreateAction();
        encounterRequest.EncounterCreateAction.PlayerId = this._player;
        encounterRequest.PlayerId = this._player;

        try
        {
            await _writeLock.WaitAsync();
            await _call.RequestStream.WriteAsync(encounterRequest);
            _writeLock.Release();

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
        encounterRequest.PlayerId = this._player;

        try
        {
            await _writeLock.WaitAsync();
            Debug.Log("UpdateEncounterAsync");

            await _call.RequestStream.WriteAsync(encounterRequest);
            _writeLock.Release();

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            throw ex;
        }
    }

    public async Task<bool> ProcessStreamResponse()
    {
        while (await _call.ResponseStream.MoveNext())
        {
            Encounter encounterResponseData = _call.ResponseStream.Current.Encounter;
            encounter = encounterResponseData;
            Debug.Log(encounter);
        }

        Debug.Log("Completed");

        return true;
    }

    public async Task GetUpdatedEncounter()
    {
        try
        {
            while (true)
            {
                await _readLock.WaitAsync();
                await ProcessStreamResponse();
                _readLock.Release();
            }
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

    public void OnApplicationQuit()
    {
        _call.RequestStream.CompleteAsync();
        _channel.ShutdownAsync().Wait();
    }

    public void OnDestroyed()
    {
        _call.RequestStream.CompleteAsync();
        _channel.ShutdownAsync().Wait();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Code.Data;
using Code.Services.SaveSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace Code.Services
{
    public class EventService : MonoBehaviour
    {
        private const string RequestHeaderName = "Content-Type";
        private const string RequestHeaderValue = "application/json; charset=UTF-8";

        public event Action<float> CooldownUpdated;
        public event Action<EventData, long> RequestDone;

        [SerializeField] private string _serverUrl;
        [SerializeField] private float _cooldownBeforeSend;

        private readonly List<UnityWebRequestAsyncOperation> _requestOperations =
            new List<UnityWebRequestAsyncOperation>();

        private SaveData _saves;
        private ISaveService _saveService;

        private Coroutine _coolDownCoroutine;

        private float _cooldownTimer;

        public void Construct(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void Enable()
        {
            _saves = _saveService.Load();
            CooldownUpdated?.Invoke(_cooldownTimer);
            CheckOldRequests();
        }
        
        public void Disable() => _saveService.Save(_saves);

        public void TrackEvent(int id, string type, string data)
        {
            CreatePostRequest(id, type, data);
            
            if (_coolDownCoroutine != null) return;
            
            _coolDownCoroutine = StartCoroutine(WaitCooldownBeforeSend());
        }

        private IEnumerator WaitCooldownBeforeSend()
        {
            while (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;
                CooldownUpdated?.Invoke(_cooldownTimer);
                yield return null;
            }

            StartCoroutine(SendAllRequests());

            _coolDownCoroutine = null;
        }

        private IEnumerator SendAllRequests()
        {
            foreach (UnityWebRequestAsyncOperation requestOperation in _requestOperations.ToList())
            {
                yield return requestOperation;

                UnityWebRequest request = requestOperation.webRequest;

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var result = JsonUtility.FromJson<EventData>(request.downloadHandler.text);

                    RequestDone?.Invoke(result, request.responseCode);

                    _requestOperations.Remove(requestOperation);
                    _saves.Data.Remove(result);
                }

                Debug.Log(request.responseCode);
            }

            _cooldownTimer = _cooldownBeforeSend;
            CooldownUpdated?.Invoke(_cooldownTimer);
        }

        private void CreatePostRequest(int id, string type, string data)
        {
            var form = new WWWForm();

            var requestData = new EventData
            {
                id = id,
                type = type,
                data = data
            };

            _saves.Data.Add(requestData);

            UnityWebRequest request = UnityWebRequest.Post(_serverUrl, form);

            byte[] dataBytes = ConvertDataToBytes(requestData);
            CreateUploadHandler(dataBytes, request);

            UnityWebRequestAsyncOperation requestOperation = request.SendWebRequest();

            _requestOperations.Add(requestOperation);
        }

        private void CreateUploadHandler(byte[] dataBytes, UnityWebRequest request)
        {
            UploadHandler uploadHandler = new UploadHandlerRaw(dataBytes);
            request.uploadHandler = uploadHandler;
            request.SetRequestHeader(RequestHeaderName, RequestHeaderValue);
        }

        private byte[] ConvertDataToBytes(EventData requestData)
        {
            string json = JsonUtility.ToJson(requestData);
            byte[] dataBytes = Encoding.UTF8.GetBytes(json);
            return dataBytes;
        }

        private void CheckOldRequests()
        {
            if (_saves.Data.Count == 0) return;

            foreach (EventData eventData in _saves.Data.ToList())
            {
                CreatePostRequest(eventData.id, eventData.type, eventData.data);
            }

            StartCoroutine(SendAllRequests());
            _cooldownTimer = _cooldownBeforeSend;
            CooldownUpdated?.Invoke(_cooldownTimer);
        }
    }
}
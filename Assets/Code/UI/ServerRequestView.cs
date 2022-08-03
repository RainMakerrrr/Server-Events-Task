using Code.Data;
using Code.Services;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class ServerRequestView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _idInputField;
        [SerializeField] private TMP_InputField _typeInputField;
        [SerializeField] private TMP_InputField _dataInputField;
        [SerializeField] private TMP_InputField _responseCodeInputField;


        private EventService _eventService;

        public void Construct(EventService eventService)
        {
            _eventService = eventService;
        }

        public void Enable() => _eventService.RequestDone += OnRequestDone;

        public void Disable() => _eventService.RequestDone -= OnRequestDone;

        private void OnRequestDone(EventData eventData, long responseCode)
        {
            _idInputField.text = eventData.id.ToString();
            _typeInputField.text = eventData.type;
            _dataInputField.text = eventData.data;
            _responseCodeInputField.text = responseCode.ToString();
        }
    }
}
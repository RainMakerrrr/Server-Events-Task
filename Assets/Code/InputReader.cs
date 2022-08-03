using Code.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _idInputField;
        [SerializeField] private TMP_InputField _typeInputField;
        [SerializeField] private TMP_InputField _dataInputField;
        [SerializeField] private Button _postButton;
        
        private EventService _eventsService;

        public void Construct(EventService eventsService)
        {
            _eventsService = eventsService;
        }

        public void Enable()
        {
            _postButton.onClick.AddListener(PostButtonClicked);
        }

        public void Disable()
        {
            _postButton.onClick.RemoveListener(PostButtonClicked);
        }

        private void PostButtonClicked()
        {
            int id = int.Parse(_idInputField.text);
            string type = _typeInputField.text;
            string data = _dataInputField.text;
            
            _eventsService.TrackEvent(id, type, data);
        }
    }
}
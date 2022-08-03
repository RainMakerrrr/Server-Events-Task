using Code.Services;
using Code.Services.SaveSystem;
using Code.UI;
using UnityEngine;

namespace Code
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private EventService _eventService;
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private CooldownView _cooldownView;
        [SerializeField] private ServerRequestView _serverRequestView;
        
        private void OnEnable()
        {
            InitEventService();
            InitInputReader();
            InitCooldownView();
            InitServerRequestView();
            
            Application.quitting += OnApplicationQuitting;
        }

        private void InitEventService()
        {
            _eventService.Construct(new SaveService());
            _eventService.Enable();
        }

        private void InitInputReader()
        {
            _inputReader.Construct(_eventService);
            _inputReader.Enable();
        }

        private void InitCooldownView()
        {
            _cooldownView.Construct(_eventService);
            _cooldownView.Enable();
        }

        private void InitServerRequestView()
        {
            _serverRequestView.Construct(_eventService);
            _serverRequestView.Enable();
        }

        private void OnApplicationQuitting()
        {
            _eventService.Disable();
            _inputReader.Disable();
            _cooldownView.Disable();
            _serverRequestView.Disable();
        }

        private void OnDisable()
        {
            Application.quitting -= OnApplicationQuitting;
            
            _eventService.Disable();
            _inputReader.Disable();
            _cooldownView.Disable();
            _serverRequestView.Disable();
        }
    }
}
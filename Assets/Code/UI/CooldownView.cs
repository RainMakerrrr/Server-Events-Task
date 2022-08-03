using System;
using System.Globalization;
using Code.Services;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class CooldownView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _cooldownText;

        private EventService _eventService;

        public void Construct(EventService eventService)
        {
            _eventService = eventService;
        }

        public void Enable() => _eventService.CooldownUpdated += OnCooldownUpdated;

        private void OnCooldownUpdated(float coolDown) =>
            _cooldownText.text = Math.Round(coolDown, 2).ToString(CultureInfo.InvariantCulture);

        public void Disable() => _eventService.CooldownUpdated -= OnCooldownUpdated;
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace CollectCreatures.LocationEvents
{
    public class LocatonEventsController : MonoBehaviour
    {
        #region VAR   
        [Space(10)]
        [SerializeField] private bool _active;
        [Space(10)]
        [SerializeField] private List<LocationEvent> _locationEvents = new List<LocationEvent>(); 
        [Space(10)]
        [SerializeField] private int _maxCountActivated = 1;
        [SerializeField] private float _startGameDelay = 20;
        [SerializeField] private float _timeBetweenEvents;

        public bool Active => _active;

        private List<LocationEvent> _activatedLocationEvents = new List<LocationEvent>();
        [Space(10)]
        [ShowOnly]
        [SerializeField] private int _countActivated = 0;
        [ShowOnly]
        [SerializeField] private float _timeEventStarted;
        [ShowOnly]
        [SerializeField] private float _timeEventFinished;
        #endregion

        #region MONO
        private void Start()
        {
            Invoke(nameof(Activate), _startGameDelay);
        }      
        #endregion

        #region FUNC    
        private void Activate()
        {
            _active = true;

            Invoke(nameof(EventsControl), 0.1f);
        }
        private void Deactivate()
        {
            _active = false;
        }
        private void EventsControl()
        {
            if (!_active)
                return;

            if (Time.time > _timeEventStarted + _timeBetweenEvents && _countActivated < _maxCountActivated)
            {
                TryActivateEvent();
            }

            Invoke(nameof(EventsControl), 1f);
        }
        private void TryActivateEvent()
        {
            LocationEvent locationEvent = FindEventForActivate();

            if (locationEvent != null)
            {
                locationEvent.StartEvent();
                locationEvent.OnLocationEventEnded += LocationEventEnd;

                _activatedLocationEvents.Add(locationEvent);

                _timeEventStarted = Time.time;
                _countActivated += 1;
            }
        }
        private LocationEvent FindEventForActivate()
        {
            for (int i = 0; i < _locationEvents.Count; i++)
            {
                if (!_locationEvents[i].Active && _locationEvents[i].CanActivated)
                    return _locationEvents[i];
            }

            return null;
        }
        #endregion

        #region CALLBAKS
        private void LocationEventEnd(LocationEvent  locationEvent)
        {
            _activatedLocationEvents.Remove(locationEvent);
            locationEvent.OnLocationEventEnded -= LocationEventEnd;

            _timeEventFinished = Time.time;
            _countActivated -= 1;
        }
        #endregion
    }
}

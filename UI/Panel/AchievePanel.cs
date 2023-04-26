using UnityEngine;
using UnityEngine.UI;

namespace VikingAchievements.UI.Panel; 

/* A custom Unity component for showing the achievement panel */
internal class AchievePanel : MonoBehaviour {
    public bool IsBusy { get; private set; }
    
    public RectTransform rect;
    public Text text;
    public AudioSource audioSource;
    
    private const float TIME_OF_MOVE = 0.35f,
                        TIME_OF_WAITING = 4f;  //Time in seconds
    private readonly Vector2 _showPivot = new(1f, 1f),
                             _hidePivot = new(-0.1f, 1f);
    private float _speed;
    private float _startTime;
    private float _distance;
    private bool _isAppearing, _isWaiting, _isDisappearing;

    private void Update() {
        if (!IsBusy) return;  //If the panel isn't busy, exit the method
        
        if (_isAppearing) {  //If the panel is appearing
            float distCovered = (Time.time - _startTime) * _speed;  //Get a covered distance
            float partOfDistance = distCovered / _distance;  //Get a part of the covered distance from the whole distance
            Vector2 tmpPivot = Vector2.Lerp(_hidePivot, _showPivot, partOfDistance);  //Get a temp panel position
            rect.pivot = tmpPivot;  //Set the temp position to the panel

            if (tmpPivot.x >= _showPivot.x) {  //If the panel already appeared
                _isAppearing = false;  //Make the panel not appear
                _isWaiting = true;  //Make the panel wait
                UpdateStartTime();  //Update the start time
            }
        } else if (_isWaiting) {  //If the panel is waiting
            if (Time.time - _startTime < TIME_OF_WAITING) return;  //If the panel hasn't been waiting long enough, exit the method
            _isWaiting = false;  //Else, make the panel not waiting
            _isDisappearing = true;  //Make the panel disappear
            PanelManager.PlatOutSound();  //Play the panel disappearing sound
            UpdateStartTime();  //Update the start time
        } else if (_isDisappearing) {  //If panel is disappearing
            /* Do the same as the panel is appearing but conversely */
            float distCovered = (Time.time - _startTime) * _speed;  //Get a covered distance
            float partOfDistance = distCovered / _distance;  //Get a part of the covered distance from the whole distance
            Vector2 tmpPivot = Vector2.Lerp(_showPivot, _hidePivot, partOfDistance);  //Get a temp panel position
            rect.pivot = tmpPivot;  //Set the temp position to the panel

            if (tmpPivot.x <= _hidePivot.x) {  //If the panel already disappeared
                _isDisappearing = false;  //Make the panel not disappear
                IsBusy = false;  //Make the panel not busy
                gameObject.SetActive(false);  //Make the panel not active
                PanelManager.NextPendedAction();  //Run the next pended action (if it exists)
            }
        }
    }

    /* Method for appearing the achievement panel and its disappearing */
    public void Appear() {
        IsBusy = true;  //Make the panel busy
        _isAppearing = true;  //Make the panel appear
        _distance = Vector2.Distance(_showPivot, _hidePivot);  //Eval the distance
        _speed = _distance / TIME_OF_MOVE;  //Set the speed
        gameObject.SetActive(true);  //Make the panel active
        PanelManager.PlayInSound();  //Play the panel appearing sound
        UpdateStartTime();  //Update the start time
    }
    
    private void UpdateStartTime() => _startTime = Time.time;
}
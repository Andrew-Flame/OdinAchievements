using UnityEngine;

namespace AwesomeAchievements.AchievePanel; 

/* A custom Unity component for showing the achievement panel */
internal class AchievementPanel : MonoBehaviour {
    public bool isBusy;
    public float distance;
    public Vector3 position, offsetPosition;

    private const float TIME_OF_MOVE = 0.35f,
                        TIME_OF_WAITING = 4f;  //Time in seconds
    private float _speed;
    private float _startTime;
    private bool _isAppearing, _isWaiting, _isDisappearing;

    private void Update() {
        if (!isBusy) return;  //If the panel isn't busy, exit the method
        
        if (_isAppearing) {  //If the panel is appearing
            float distCovered = (Time.time - _startTime) * _speed;  //Get a covered distance
            float partOfDistance = distCovered / distance;  //Get a part of the covered distance from the whole distance
            Vector3 tempPos = Vector3.Lerp(offsetPosition, position, partOfDistance);  //Get a temp panel position
            PanelManager.Reposition(tempPos);  //Set the temp position to the panel
            
            if (tempPos.x <= position.x) {  //If the panel already appeared
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
            float partOfDistance = distCovered / distance;  //Get a part of the covered distance from the whole distance
            Vector3 tempPos = Vector3.Lerp(position, offsetPosition, partOfDistance);  //Get a temp panel position
            PanelManager.Reposition(tempPos);  //Set the temp position to the panel

            if (tempPos.x >= offsetPosition.x) {  //If the panel already disappeared
                _isDisappearing = false;  //Make the panel not disappear
                isBusy = false;  //Make the panel not busy
                gameObject.SetActive(false);  //Make the panel not active
                PanelManager.NextPendedAction();  //Run the next pended action (if it exists)
            }
        }
    }

    /* Method for appearing the achievement panel and its disappearing */
    public void Appear() {
        isBusy = true;  //Make the panel busy
        _isAppearing = true;  //Make the panel appear
        _speed = distance / TIME_OF_MOVE;  //Set the speed
        gameObject.SetActive(true);  //Make the panel active
        PanelManager.PlayInSound();  //Play the panel appearing sound
        UpdateStartTime();  //Update the start time
    }
    
    private void UpdateStartTime() => _startTime = Time.time;
}
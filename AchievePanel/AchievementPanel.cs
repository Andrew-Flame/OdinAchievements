using UnityEngine;

namespace AwesomeAchievements.AchievePanel; 

internal class PanelComponent : MonoBehaviour {
    public bool isBusy;
    public float distance;
    public Vector3 position, offsetPosition;

    private const float TIME_OF_MOVE = 0.35f,
                        TIME_OF_WAITING = 4f;  //Time in seconds
    private float _speed;
    private float _startTime;
    private bool _isAppearing, _isWaiting, _isDisappearing;

    private void Update() {
        if (_isAppearing) {
            float distCovered = (Time.time - _startTime) * _speed;
            float partOfDistance = distCovered / distance;
            Vector3 tempPos = Vector3.Lerp(offsetPosition, position, partOfDistance);
            PanelHandler.panelRect.position = tempPos;
            
            if (tempPos.x <= position.x) {
                _isAppearing = false;
                _isWaiting = true;
                UpdateStartTime();
            }
        } else if (_isWaiting) {
            if (Time.time - _startTime < TIME_OF_WAITING) return;
            _isWaiting = false;
            _isDisappearing = true;
            UpdateStartTime();
        } else if (_isDisappearing) {
            float distCovered = (Time.time - _startTime) * _speed;
            float partOfDistance = distCovered / distance;
            Vector3 tempPos = Vector3.Lerp(position, offsetPosition, partOfDistance);
            PanelHandler.panelRect.position = tempPos;

            if (tempPos.x >= offsetPosition.x) {
                _isDisappearing = false;
                isBusy = false;
                gameObject.SetActive(false);
                PanelHandler.RunNextPendingAction();
            }
        }
    }

    public void Appear() {
        isBusy = true;
        _isAppearing = true;
        _speed = distance / TIME_OF_MOVE;
        gameObject.SetActive(true);
        UpdateStartTime();
    }
    
    private void UpdateStartTime() => _startTime = Time.time;
}
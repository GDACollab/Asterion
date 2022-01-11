using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Spacefighter
{
    public class TestArcadePlayer : MonoBehaviour
    {
        // External
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _frontTransform;
        private Transform _playerTransform;
        private Rigidbody2D _rigidbody2D;

        // Exposed variables
        [SerializeField] private float _moveAccel;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _slowdownDecel;
        [SerializeField] private float _stopThreshold;
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private float _fireRate;

        [SerializeField] private GameObject _playerBullet;

        // Internal
        private Vector2 _currentVelocity;
        private Vector2 _inputVector;

        private bool _characterEnabled = false;
        private bool _canShoot = true;

        public bool characterEnabled
        {
            private set { characterEnabled = _characterEnabled; }
            get { return _characterEnabled; }
        }

        public void Construct()
        {
            _playerTransform = GetComponent<Transform>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _currentVelocity = Vector2.zero;
        }

        private void Update()
        {
            if(_characterEnabled)
            {
                // Mouse0 being pressed
                GetInputVector();
                if (Input.GetButton("Fire1"))
                {
                    Move();
                    RotateToward();
                }
                // Mouse0 not being pressed
                else
                {
                    Slowdown();
                }

                if (Input.GetButton("Fire2"))
                {
                    RotateToward();
                    if (_canShoot)
                    {
                        Shoot();
                        StartCoroutine(ShootCooldown());
                    }
                }

                // Set velocity
                _rigidbody2D.velocity = _currentVelocity;
            }
        }

        private void GetInputVector()
        {
            Vector3 playerPosition = _playerTransform.position;
            Vector3 mousePosition = _playerCamera
                .ScreenToWorldPoint(Input.mousePosition);
            _inputVector = new Vector2(mousePosition.x - playerPosition.x
                , mousePosition.y - playerPosition.y);
            _inputVector.Normalize();
        }

        private void Move()
        {
            Vector2 newMove = _inputVector * _moveAccel * Time.deltaTime;
            _currentVelocity += newMove;

            _currentVelocity = Vector2.ClampMagnitude(_currentVelocity, _maxSpeed);
        }

        private void Slowdown()
        {
            float currentSpeed = _currentVelocity.magnitude;
            float newSpeed = currentSpeed - _slowdownDecel * Time.deltaTime;
            if (newSpeed < _stopThreshold)
            {
                _currentVelocity = Vector2.zero;
            }
            else
            {
                _currentVelocity.Normalize();
                _currentVelocity *= newSpeed;
            }
        }

        private void RotateToward()
        {
            float targetRotation = Mathf.Atan2(_inputVector.y, _inputVector.x) * Mathf.Rad2Deg;

            //// Experimenting with dotProduct
            //int rotateDirection = FindRotationDirection(currentRotation, targetRotation);
            //// How close ship forward (transform.right) is to the input direction
            //// -1 being opposite, 0 being perpendicular, 1 being facing same direction
            //float dotProduct = Vector2.Dot(_playerTransform.right, _inputVector);
            //// Rescale dot product range of -1 to 1 to 0 to 1
            //float rotateSpeedMult = UtilityFunctions.Rescale(-1, 1, 1, 0.1f, dotProduct);

            _playerTransform.rotation = Quaternion.Slerp(
                transform.rotation, Quaternion.Euler(0, 0, targetRotation)
                , _rotateSpeed * Time.deltaTime);
        }

        int FindRotationDirection(float current, float target)
        {
            float difference = target - current;
            // left
            if (difference > 180) return -1;
            // right
            else return 1;
        }

        private IEnumerator ShootCooldown()
        {
            _canShoot = false;
            yield return new WaitForSeconds(_fireRate);
            _canShoot = true;
        }

        private void Shoot()
        {
            Vector2 thisDirection = new Vector2(
                _frontTransform.position.x - _playerTransform.position.x
                , _frontTransform.position.y - _playerTransform.position.y).normalized;

            GameObject thisBullet = Instantiate(_playerBullet, _playerTransform.position
                , Quaternion.identity);
            thisBullet.GetComponent<PlayerBullet>()
                .Construct(thisDirection);
        }

        public void ToggleCharacterEnable()
        {
            _characterEnabled = !_characterEnabled;
        }
    }
}
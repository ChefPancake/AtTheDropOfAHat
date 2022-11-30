using DropOfAHat.Game;
using DropOfAHat.Player;
using UnityEngine;

namespace DropOfAHat.Hat {
    internal interface IHatState {
        bool CanInteract => false;
        bool CanThrow => false;

        IHatState Catch(GameObject caughtBy, bool canThrow) => this;
        IHatState Throw() => this;
        IHatState LeaveObject(GameObject leaving) => this;
        IHatState Pop() => this;
        IHatState FinishPopping() => this;
    }
    internal class HatInAir : IHatState {
        public bool CanInteract => true;

        internal HatInAir() {}

        public static IHatState Spawn() =>
            new HatInAir();

        public IHatState Catch(GameObject caughtBy, bool canThrow) =>
            new HatOnObject(caughtBy, canThrow);

        public IHatState Pop() =>
            new HatPopping();
    }

    internal class HatPopping : IHatState {
        internal HatPopping() {}

        public IHatState FinishPopping() => 
            new HatPopped();
    }

    internal class HatPopped : IHatState {
        internal HatPopped() {}
    }

    internal class HatOnObject : IHatState {
        public GameObject OnObject { get; }
        public bool CanThrow { get; }

        internal HatOnObject(GameObject onObject, bool canThrow) {
            OnObject = onObject;
            CanThrow = canThrow;
        }

        public static IHatState SpawnOn(GameObject onObject, bool canThrow) =>
            new HatOnObject(onObject, canThrow);

        public IHatState Throw() =>
            CanThrow 
            ? new HatLeavingObject(OnObject)
            : this;
    }

    internal class HatLeavingObject : IHatState {
        public bool CanInteract => true;

        public GameObject LeavingObject { get; }

        internal HatLeavingObject(GameObject leaving) =>
            LeavingObject = leaving;

        public IHatState LeaveObject(GameObject leaving) =>
            leaving == LeavingObject
            ? new HatInAir()
            : this;

        public IHatState Pop() =>
            new HatPopping();
    }
    
    public class Hat : MonoBehaviour {
        private const string IS_LANDING_ANIMATION_STATE = "IsLanding";
        private const string IS_ON_OBJECT_ANIMATION_STATE = "IsOnPlayer";
        private const string IS_POPPED_ANIMATION_STATE = "IsPopped";
        private const string VELOCITY_ANIMATION_STATE = "Velocity";

        [SerializeField]
        private GameObject _player;
        [SerializeField]
        private Vector3 _offsetOnPlayer;

        private Rigidbody2D _rigidBody;
        private Collider2D _collider;
        private GameEvents _events;
        private Animator _animator;
        private ParticleSystem _particles;
        private AudioSource _hurtAudio;

        private IHatState _hatState;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _animator = GetComponentInChildren<Animator>();
            _hurtAudio = GetComponent<AudioSource>();
            Spawn();
            _events.Subscribe<PlayerThrow.HatThrown>(OnThrown);
            _events.Subscribe<LevelStart.LevelLoadedEvent>(OnLevelLoad);
        }

        private void FixedUpdate() {
            if (_hatState is HatInAir || _hatState is HatLeavingObject) {
                SetRotation();
            }
        }

        private void SetRotation() {
            var velocity = _rigidBody.velocity;
            var up = Vector3.up;
            var angle = Vector3.Angle(up, velocity);
            angle = velocity.x > 0
                ? -angle
                : angle;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            _animator.SetFloat(
                VELOCITY_ANIMATION_STATE,
                velocity.magnitude);
        }

        private void Update() {
            if (_hatState is HatOnObject hat && hat.OnObject) {
                transform.position = hat.OnObject.transform.position + _offsetOnPlayer;
                transform.rotation = Quaternion.identity;
            }
        }

        public void Spawn() {
            _hatState = HatOnObject.SpawnOn(_player, true);
            Catch(_player, true);
        }

        public void Catch(GameObject caughtBy, bool canThrow) {
            _hatState = _hatState.Catch(caughtBy, canThrow);
            SetPhysics(_hatState.CanInteract);
            if (_hatState is HatOnObject) {
                _animator.SetBool(IS_LANDING_ANIMATION_STATE, true);
                _animator.SetBool(IS_ON_OBJECT_ANIMATION_STATE, true);
                _animator.SetBool(IS_POPPED_ANIMATION_STATE, false);
                _animator.SetFloat(VELOCITY_ANIMATION_STATE, 0f);
                _events.Send(new CaughtEvent(caughtBy));
            }
        }

        private void OnThrown(PlayerThrow.HatThrown thrown) {
            if (_hatState.CanThrow) {
                _hatState = _hatState.Throw();
                _animator.SetFloat(VELOCITY_ANIMATION_STATE, thrown.ThrowVec.magnitude);
                _animator.SetBool(IS_ON_OBJECT_ANIMATION_STATE, false);
                _animator.SetBool(IS_LANDING_ANIMATION_STATE, false);
                SetPhysics(_hatState.CanInteract);
                _rigidBody.velocity = thrown.ThrowVec;
            }
        }

        private void OnLevelLoad(LevelStart.LevelLoadedEvent _) =>
            Spawn();

        private void SetPhysics(bool enabled) {
            _collider.enabled = enabled;
            _rigidBody.simulated = enabled;
            if (enabled) {
                _rigidBody.position = transform.position;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            _hatState = _hatState.LeaveObject(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.TryGetComponent<HatCatcher>(out var catcher)) {
                Debug.Log(catcher.CanThrowFrom);
                Catch(other.gameObject, catcher.CanThrowFrom);
            } else if (other.gameObject.CompareTag("World") || other.gameObject.CompareTag("Enemy")) {
                Pop();
            }
        }

        private void Pop() {
            _hatState = _hatState.Pop();
            if (_hatState is HatPopping) {
                SetPhysics(_hatState.CanInteract);
                _animator.SetBool(IS_POPPED_ANIMATION_STATE, true);
                _events.Send(new DroppedEvent());
                _hurtAudio.Play();
            }
        }

        public struct DroppedEvent { }

        public struct CaughtEvent { 
            public GameObject CaughtBy { get; }

            public CaughtEvent(GameObject caughtBy) =>
                CaughtBy = caughtBy;
        }
    }
}
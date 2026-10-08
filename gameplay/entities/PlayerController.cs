using Godot;
using Nix.Core.Entities;
using Nix.Core.Input;
using Nix.Core.Stats;
using Nix.Services.Bootstrap;
using Nix.Services.Input;
namespace Nix.Gameplay.Entities;
public partial class PlayerController : CharacterBody2D
{
    private enum State { Idle, Move, Roll }
    [Export] public float RollDuration = 0.4f;
    [Export] public float RollSpeed = 400f;
    [Export] public float RollBufferTime = 0.18f;
    private float _rollBuffer;
    private Vector2 _lastMoveDir = Vector2.Zero;
    private State _state = State.Idle;
    private AnimatedSprite2D _legsSprite = null!;
    private AnimatedSprite2D _torsoSprite = null!;
    private AnimatedSprite2D? _headSprite;
    private AnimatedSprite2D _rollSprite = null!;
    private Node2D _upperBody = null!;
    private Node2D _weaponPivot = null!;
    private Sprite2D _weaponSprite = null!;
    public StatBlock Stats { get; private set; } = null!;
    private float _rollTimer;
    private Vector2 _rollDirection;
    private Vector2 _lastAim = Vector2.Right;
    public bool Invulnerable { get; private set; }
    private bool _referencesValid = true;
    public override void _Ready()
    {
        _legsSprite = RequireNode<AnimatedSprite2D>("Legs/LegsSprite")!;
        _torsoSprite = RequireNode<AnimatedSprite2D>("UpperBody/TorsoSprite")!;
        _headSprite = GetNodeOrNull<AnimatedSprite2D>("UpperBody/HeadSprite");
        _rollSprite = RequireNode<AnimatedSprite2D>("RollSprite")!;
        _upperBody = RequireNode<Node2D>("UpperBody")!;
        _weaponPivot = RequireNode<Node2D>("UpperBody/WeaponPivot")!;
        _weaponSprite = RequireNode<Sprite2D>("UpperBody/WeaponPivot/WeaponSprite")!;

        _rollSprite.Visible = false;
        Stats = new StatBlock(StatValues.Empty);
    }
    private T? RequireNode<T>(string path) where T : class
    {
        var node = GetNodeOrNull<T>(path);
        if (node is null)
        {
            _referencesValid = false;
            GD.PushError($"[PlayerController] Не найдена нода: '{path}'. Проверь имена и иерархию в Player.tscn.");
        }
        return node;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!_referencesValid)
            return;
        var input = this.Svc<InputService>().Current;
        var dt = (float)delta;
        _rollBuffer = Mathf.Max(0f, _rollBuffer - dt);
        if (input.RollPressed)
        {
            _rollBuffer = RollBufferTime;
        }
        switch (_state)
        {
            case State.Idle:
            case State.Move:
                HandleMovement(input);
                break;
            case State.Roll:
                HandleRoll(dt);
                break;
        }
        MoveAndSlide();
        if (_state != State.Roll)
            UpdateLegsAnimation(input.Move);
        UpdateAim(input);
    }
    private void HandleMovement(InputSnapshot input)
    {
        Velocity = input.Move * Stats.CurrentValue.MoveSpeed;
        _state = input.Move.LengthSquared() > 0.01f ? State.Move : State.Idle;
        if (input.Move != Vector2.Zero)
        {
            _lastMoveDir = input.Move.Normalized();
        }
        if (_rollBuffer > 0f)
        {
            var dir = input.Move!= Vector2.Zero ? input.Move.Normalized() : _lastMoveDir;
            if (dir!= Vector2.Zero)
            {
                EnterRoll(dir);
                _rollBuffer = 0f;
            }
        }

    }
    private void EnterRoll(Vector2 direction)
    {
        _state = State.Roll;
        _rollTimer = RollDuration;
        _rollDirection = direction;
        Invulnerable = true;
        _legsSprite.Visible = false;
        _upperBody.Visible = false;
        _rollSprite.Visible = true;
        _rollSprite.Play(DirectionName("roll", direction));
    }
    private void HandleRoll(float dt)
    {
        Velocity = _rollDirection * RollSpeed;
        _rollTimer -= dt;
        if (_rollTimer <= 0f)
            ExitRoll();
    }
    private void ExitRoll()
    {
        _state = State.Idle;
        Invulnerable = false;
        _rollSprite.Visible = false;
        _legsSprite.Visible = true;
        _upperBody.Visible = true;
    }
    private void UpdateLegsAnimation(Vector2 move)
    {
        if (move.LengthSquared() < 0.01f)
        {
            _legsSprite.Play("idle_down");
            return;
        }
        _legsSprite.Play(DirectionName("walk", move));
    }
    private void UpdateAim(InputSnapshot input)
    {

        if (_state == State.Roll)
            return;
        var aim = ResolveAimDirection(input);
        if (aim != Vector2.Zero)
            _lastAim = aim;
        aim = _lastAim;
        var torsoAnim = DirectionName("aim", aim);
        _torsoSprite.Play(torsoAnim);
        _headSprite?.Play(torsoAnim);
        _weaponPivot.Rotation = Mathf.Atan2(aim.Y, aim.X);
        _weaponSprite.FlipV = aim.X < 0;
        _weaponPivot.Rotation = Mathf.Atan2(aim.Y, aim.X);
        _weaponPivot.ZIndex = aim.Y < 0 ? -1 : 1;
        _weaponSprite.FlipV = aim.X < 0;
    }
    private Vector2 ResolveAimDirection(InputSnapshot input)
    {
        if (input.AimFromPointer)
        {
            var worldPointer = GetCanvasTransform().AffineInverse() * input.PointerViewportPos;
            return (worldPointer - GlobalPosition).Normalized();
        }
        return input.AimStick;
    }
    private static string DirectionName(string prefix, Vector2 dir)
    {
        if (Mathf.Abs(dir.Y) > Mathf.Abs(dir.X))
            return prefix + (dir.Y > 0 ? "_down" : "_up");
        return prefix + (dir.X > 0 ? "_right" : "_left");
    }
}
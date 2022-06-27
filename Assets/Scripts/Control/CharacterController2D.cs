using UnityEngine;
using Sirenix.OdinInspector;


/// <summary>
/// 2D平台游戏角色控制器:
/// 检测碰撞,控制Sprite的翻转,以及Sprite的移动
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    [ShowInInspector, DisplayAsString]
    public bool IsGrounded { get; protected set; }

    public bool JustGotGrounded { get; protected set; }

    [ShowInInspector, DisplayAsString]
    public bool IsCollidingLeft { get; protected set; }

    [ShowInInspector, DisplayAsString]
    public bool IsCollidingRight { get; protected set; }

    public bool IsCollidingSides { get; protected set; }

    [FoldoutGroup("物理碰撞检测")]
    [Header("WallCheck")]
    public LayerMask wallMask = default;

    [FoldoutGroup("物理碰撞检测")]
    public Transform wallCheckPos;


    private Vector2 horizontalRayCastFromBottom;
    private Vector2 horizontalRayCastToTop;

    [FoldoutGroup("物理碰撞检测")]
    [Header("GroundedCheck")]
    public LayerMask groundMask = default;
    [FoldoutGroup("物理碰撞检测")]
    public Vector2 groundedCheckPoint = default;
    [FoldoutGroup("物理碰撞检测")]
    public float groundedCheckRadius = 0.2f;


    public Vector2 Velocity { get; protected set; }


    [HideInInspector] public Collider2D myCollider;
    private Rigidbody2D rb;





    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        rb = myCollider.attachedRigidbody;
        if (modelTrans == null)
        {
            modelTrans = GetComponentInChildren<SpriteRenderer>().transform;
        }
    }
    private void FixedUpdate()
    {
        //禁止玩家推敌人和敌人推玩家
        if (this.tag=="Player"&&wallCheckPos!=null&&Physics2D.OverlapCircle(wallCheckPos.position,0.2f,wallMask)&& Velocity.x>0
            || this.tag == "Enemy" && wallCheckPos != null && Physics2D.OverlapCircle(wallCheckPos.position, 0.2f, wallMask) && Velocity.x < 0)
        {
            if (GetComponentInParent<Enemy>()&& GetComponentInParent<Enemy>().isUnInterrupt)
            {
                return;
            }
            Velocity = Vector2.zero;
        }
        //HandleCollision();
        HandleMovement();
    }


    private void HandleMovement()
    {
        rb.MovePosition(rb.position + Velocity * Time.fixedDeltaTime);
        Velocity = Vector2.zero;
    }

    /*
    #region 物理检测部分

    /// <summary>
    /// 水平方向的射线检测
    /// </summary>
    /// <param name="rayDirection">1或-1</param>
    /// <returns></returns>
    private bool CastRayToSides(float rayDirection, LayerMask layerMask)
    {
        Vector2 bound = myCollider.bounds.size;
        horizontalRayCastFromBottom = (Vector2)myCollider.bounds.center - bound.y / 2 * Vector2.up;
        horizontalRayCastToTop = (Vector2)myCollider.bounds.center + bound.y / 2 * Vector2.up;

        float rayCastLength = bound.x / 2 + rayCastLengthOffset;

        if (rayDirection > 0)
            rayDirection = 1;
        else if (rayDirection < 0)
            rayDirection = -1;

        for (int i = 0; i < numberOfHorizontalRays; i++)
        {
            Vector2 rayStartPos = Vector2.Lerp(horizontalRayCastFromBottom, horizontalRayCastToTop, (float)(i + 1) / (float)(numberOfHorizontalRays + 1));
            //Debug.DrawRay(rayStartPos, Vector2.right * rayDirection, Color.blue, rayCastLength);
            if (Physics2D.Raycast(rayStartPos, Vector2.right * rayDirection, rayCastLength, layerMask))
            {
                return true;
            }
        }
        return false;
    }

    public void ResetGravity()
    {
        GravityScale = gravityScale;
    }

    private bool CheckGrounded(Vector3 agentPos)
    {
        return Physics2D.OverlapCircle(agentPos + new Vector3(groundedCheckPoint.x, groundedCheckPoint.y, 0), groundedCheckRadius, groundMask);
    }
    private void HandleCollision()
    {
        JustGotGrounded = (!IsGrounded) && CheckGrounded(this.transform.position);
        IsGrounded = CheckGrounded(this.transform.position);
        IsCollidingLeft = CastRayToSides(-1, wallMask);
        IsCollidingRight = CastRayToSides(1, wallMask);
    }
    #endregion
    */

    #region 移动控制部分
    [SerializeField]
    protected Transform modelTrans;

    public bool isFacingRight = true;
    public int Forward
    {
        get
        {
            if (isFacingRight)
                return 1;
            else
                return -1;
        }
    }

    /// <summary>
    /// Force the character face to a direction
    /// </summary>
    /// <param name="xDirection">Face right when this float bigger than 0</param>
    public virtual void FaceTo(float xDirection)
    {
        if (xDirection == 0) { return; }

        if (xDirection > 0 && !isFacingRight)
        {
            Filp();
            isFacingRight = true;
        }
        else if (xDirection < 0 && isFacingRight)
        {
            Filp();
            isFacingRight = false;
        }
    }

    public virtual void FaceTo(Vector3 target)
        => FaceTo(target.x - this.transform.position.x);


    /// <summary>
    /// Flips the character and its dependencies horizontally
    /// 水平翻转SpriteRender
    /// </summary>
    protected virtual void Filp()
    {
        if (modelTrans != null)
        {
            modelTrans.transform.localScale = Vector3.Scale
                (modelTrans.transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    /// <summary>
    /// Moving character's rigidbody2D horizontally, direction is current direction
    /// (You can simply set speed to zero in this method when you want to stop moving)
    /// </summary>
    /// <param name="horizontalSpeed"></param>
    public void MoveForward(float horizontalSpeed)
        => SetXSpeed(horizontalSpeed * Forward);


    public void MoveTowards(float horizontalSpeed, float direction)
    {
        FaceTo(direction);
        SetXSpeed(horizontalSpeed * HandlerFloatDirection(direction));
    }


    public void MoveBackward(float horizontalSpeed)
    {
        SetXSpeed(horizontalSpeed * (-Forward));
    }

    public void MoveBackward(float horizontalSpeed, float backDirection)
    {
        FaceTo(-backDirection);
        SetXSpeed(horizontalSpeed * HandlerFloatDirection(backDirection));
    }

    public void Stop()
    {
        Velocity = Vector2.zero;
    }

    private int HandlerFloatDirection(float direction)
    {
        if (direction > 0)
            return 1;
        else if (direction < 0)
            return -1;
        else
            return 0;
    }

    //对速度进行修改

    public void SetXSpeed(float xSpeed)
    {
        SetVelocity(new Vector2(xSpeed, Velocity.y));
    }

    public void SetYSpeed(float ySpeed)
    {
        SetVelocity(new Vector2(Velocity.x, ySpeed));
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        Velocity = newVelocity;
    }


    #endregion
}

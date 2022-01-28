using UnityEngine;

/*
 * Fighter AI movement.
 *
 * Developer: Jonah Ryan
 */

public class scr_fighter_move : MonoBehaviour
{
    public float speed;

    public int Ai_Type;

    private Rigidbody2D m_Rigidbody;
    
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 playerPos = scr_find_player.Get_Player_Pos(Ai_Type);
        Vector2 dir = playerPos - m_Rigidbody.position;

        // Move and rotate towards player
        m_Rigidbody.MovePosition(Vector2.MoveTowards(m_Rigidbody.position, playerPos, Time.deltaTime * speed));

        m_Rigidbody.rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}
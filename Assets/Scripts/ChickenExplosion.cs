using UnityEngine;
using System.Collections;

public class ChickenExplosion : MonoBehaviour {

    private float m_FlyingChickenDuration;
    private float m_ChickenLaunched;    

    // Use this for initialization
    void Start()
    {
        m_FlyingChickenDuration = 1f;
        m_ChickenLaunched = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_ChickenLaunched += Time.deltaTime;
        if (m_ChickenLaunched >= m_FlyingChickenDuration)
            EndExplosion();
    }

    private void EndExplosion()
    {
        //lancer l'animation d'explosion
        //poser le cadavre
        Destroy(this.gameObject);
    }
}

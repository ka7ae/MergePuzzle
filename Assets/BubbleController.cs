using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    //�V�[���f�B���N�^�[
    public MergePuzzleSceneDirector SceneDirector;
    //color
    public int ColorType;
    //�}�[�W�σt���O
    public bool IsMerged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //��ʊO�ɗ����������
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    //�����蔻�肪����������Ă΂��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�o�u������Ȃ�
        BubbleController bubble = collision.gameObject.GetComponent<BubbleController>();
        if(!bubble) return;

        //���̂�����
        SceneDirector.Merge(this, bubble);
    }
}

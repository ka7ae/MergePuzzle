using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MergePuzzleSceneDirector : MonoBehaviour
{
    //�A�C�e���̃v���n�u
    [SerializeField] List<BubbleController> prefabBubbles;
    //UI
    [SerializeField] TextMeshProUGUI textScore;
    [SerializeField] GameObject panelResult;
    //Audio
    [SerializeField] AudioClip seDrop;
    [SerializeField] AudioClip seMerge;

    //score
    int score;
    //���݂̃A�C�e��
    BubbleController currentBubble;
    //�����ʒu
    const float SpawnItemY = 3.5f;
    //Audio�Đ����u
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //�T�E���h�Đ��p
        audioSource = GetComponent<AudioSource>();
        //���U���g��ʔ�\��
        panelResult.SetActive(false);

        //�ŏ��̃A�C�e���𐶐�
        //IEnumerator���Ăяo���̂�StartCoroutine���g���@����̓Z�b�g�Ŋo���Ă����Ƃ悫
        StartCoroutine(SpawnCurrentItem());
    }

    // Update is called once per frame
    void Update()
    {
        //�A�C�e�����Ȃ���΂������牺�̏����͂��Ȃ�
        if (!currentBubble) return;

        //�}�E�X�̃|�W�V�����i�X�N���[���̍��W�j���烏�[���h���W�ɕϊ�
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //�����W���}�E�X�ɍ��킹��
        Vector2 bubblePosition = new Vector2(worldPoint.x, SpawnItemY);
        currentBubble.transform.position = bubblePosition;

        //�^�b�`����
        if(Input.GetMouseButtonUp(0))�@//�{�^���������ꂽ���Ɂ@GetMouseButtonUp
        {
            //�d�͂��Z�b�g���ăh���b�v
            currentBubble.GetComponent<Rigidbody2D>().gravityScale = 1; 
            //�����A�C�e�����Z�b�g
            currentBubble = null;
            //���̃A�C�e��
            StartCoroutine(SpawnCurrentItem());
            //SE�Đ�
            audioSource.PlayOneShot(seDrop);
        }
    }



    //�A�C�e������
    BubbleController SpawnItem(Vector2 position, int colorType = -1)
    {
        //�F�����_��
        int index = UnityEngine.Random.Range(0, prefabBubbles.Count / 2);

        //�F�̎w�肪����Ώ㏑��
        if(0 < colorType)
        {
            index = colorType;
        }

        //����
        BubbleController bubble = Instantiate(prefabBubbles[index], position, Quaternion.identity);

        //�K�{�f�[�^�Z�b�g
        bubble.SceneDirector = this;
        bubble.ColorType = index;

        return bubble;
    }



    //�����A�C�e������
    IEnumerator SpawnCurrentItem()�@
    {
        //�w�肳�ꂽ�b���҂�
        yield return new WaitForSeconds(1.0f);
        //�������ꂽ�A�C�e����ێ�����
        currentBubble = SpawnItem(new Vector2(0, SpawnItemY));
        //�����Ȃ��悤�ɏd�͂�0�ɂ���
        currentBubble.GetComponent<Rigidbody2D>().gravityScale = 0;
    }



    //�A�C�e�������̂�����
    public void Merge(BubbleController bubbleA, BubbleController bubbleB)
    {

        //���쒆�̃A�C�e���ƂԂ�������Q�[���I�[�o�[
        if(currentBubble == bubbleA || currentBubble == bubbleB) // || = or
        {
            //����Update�ɓ���Ȃ��悤�ɂ���
            enabled = false;
            //���U���g�p�l���\��
            panelResult.SetActive(true);

            return;
        }

        //�}�[�W��
        if (bubbleA.IsMerged || bubbleB.IsMerged) return;

        //�Ⴄ�F
        if (bubbleA.ColorType != bubbleB.ColorType) return;

        //���ɐ�������F���p�ӂ��Ă��郊�X�g�̍ő吔�𒴂���
        int nextColor = bubbleA.ColorType + 1;
        if (prefabBubbles.Count - 1 < nextColor) return;

        //2�_�Ԃ̒��S �@�@Vector2.Lerp 2�_�Ԃ̊Ԃ̓���̈ʒu��Ԃ��Ă����֐��@0.5f�Ȃ�^��
        Vector2 lerpPosition = Vector2.Lerp(bubbleA.transform.position, bubbleB.transform.position, 0.5f);

        //�V�����A�C�e���𐶐�
        BubbleController newBubble = SpawnItem(lerpPosition, nextColor);

        //�}�[�W�σt���O�I��
        bubbleA.IsMerged = true;
        bubbleB.IsMerged = true;

        //�V�[������폜
        Destroy(bubbleA.gameObject);
        Destroy(bubbleB.gameObject);

        //�_���v�Z�ƕ\���X�V
        score += newBubble.ColorType * 10;
        textScore.text = "" + score;

        //SE�Đ�
        audioSource.PlayOneShot(seMerge);
    }



    //���g���C�{�^��
    public void OnClickRetry()
    {
        SceneManager.LoadScene("MergePuzzleScene");
    }

}

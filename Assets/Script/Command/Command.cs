using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Command : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Constant constant;   //�ؽ�Ʈ
    [SerializeField] protected Status status;        //�Ҹ�
    [SerializeField] protected bool isOn = true;     //Ȱ��ȭ ����

    protected List<Command> allChildCommands = new List<Command>();

    private ListMenu listMenu;
    private RectTransform rectTrans;
    protected Image image;
    protected BoxCollider col;
    protected Text text;

    private bool canMove = true;    //�̵� ���� ����
    protected bool canActive = true;//��ȣ�ۿ� ���ɿ���
    protected string commandName;   //�̸�

    public Status MyStatus { get => status; set => status = value; }
    public RectTransform RectTrans { get { return rectTrans; } set { rectTrans = value; } }
    public string CommandName { get { return commandName; } set { commandName = value; text.text = commandName; } }
    public virtual bool IsOn { get { return isOn; } set { isOn = value; gameObject.SetActive(isOn); } }

    public Image Images { get { return image; } set { image = value; } }
    protected virtual void Awake()
    {
        Command[] ChildCommands = GetComponentsInChildren<Command>(true);

        foreach (Command childCommand in ChildCommands)
            if (childCommand != this)
                allChildCommands.Add(childCommand);

        listMenu = transform.GetChild(1).GetComponent<ListMenu>();
        rectTrans = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();
        col = GetComponent<BoxCollider>();

        commandName = text.text;
        ActiveOnOff(true);
    }

    protected virtual void Start()
    {
        foreach (Command childCommand in allChildCommands)
            childCommand.canMove = false;

        col.size = new Vector3(RectTrans.sizeDelta.x, RectTrans.sizeDelta.y, 1);
        listMenu.IsActive(false);
    }

    private void OnEnable()
    {
        gameObject.SetActive(IsOn);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //�����Ѱ��� ����, ������ �� �ְ�, ��ȣ�ۿ� �����ϸ�
        if (HandleManager.instance.SelectMoveCommand == null && canMove && canActive) 
            HandleManager.instance.TakeMoveable(this);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (allChildCommands.Count == 0)
        {

        }

        if (HandleManager.instance.SelectMoveCommand == this)
            HandleManager.instance.SelectMoveCommand = null;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (HandleManager.instance.SelectMoveCommand == null && canActive)
            listMenu.IsActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        listMenu.IsActive(false);
    }

    public virtual void Excute()
    {
        Player.instance.SetStatus(status);
    }

    public virtual void CommandActive(string name, bool on)
    {
        foreach (Command childCommand in allChildCommands)
            if (childCommand.CommandName == name)
                childCommand.IsOn = on;
    }

    public void CommandActiveAll(bool on)
    {
        foreach (Command childCommand in allChildCommands)
            foreach (Transform immediateChild in childCommand.transform)
                childCommand.IsOn = on;
    }

    public void ActiveOnOff(bool on)
    {
        if (on)
        {
            canActive = true;
            col.isTrigger = false;
            image.color = new Color(1f, 1f, 1f, 0.5f);  //���
        }
        else
        {
            canActive = false;
            col.isTrigger = true;
            image.color = new Color(1f, 0f, 0f, 0.5f);  //������
        }
    }
}

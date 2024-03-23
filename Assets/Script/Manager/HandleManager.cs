using UnityEngine;

public class HandleManager : MonoBehaviour
{
    public static HandleManager instance;

    private ItemAreaUI moveArea;
    private Vector2 initPosition;

    private Command selectMoveCommand;
    public Command SelectMoveCommand
    {
        get { return selectMoveCommand; }
        set
        {
            if (selectMoveCommand is Item && value == null)
            {
                Area area = moveArea.FindArea(selectMoveCommand.RectTrans.position);

                if (area)
                {
                    if (area.SetItem(selectMoveCommand as Item, false) == false)
                    {
                        selectMoveCommand.RectTrans.anchoredPosition = initPosition;
                        Player.instance.ShowIntroduce(Constant.fullWeight);
                    }
                }
            }

            selectMoveCommand = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if (selectMoveCommand)
        {
            Vector2 pos = Input.mousePosition;

            if (moveArea.FindArea(pos))
            {
                selectMoveCommand.RectTrans.position = pos;
            }
            else
            {
                Debug.Log("마우스가 범위 밖으로 나감");
            }
        }
    }

    public void TakeMoveable(Command moveable)
    {
        moveArea = moveable.GetComponentInParent<ItemAreaUI>();

        if (moveArea)
        {
            selectMoveCommand = moveable;
            initPosition = selectMoveCommand.RectTrans.anchoredPosition;
        }
        else
        {
            Debug.Log("이동 범위의 부모 오브젝트가 없음");
        }
    }
}

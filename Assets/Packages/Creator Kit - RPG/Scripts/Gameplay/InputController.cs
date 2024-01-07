using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;


/// <summary>
/// Sends user input to the correct control systems.
/// </summary>
public class InputController : MonoBehaviour
{
    public float stepSize = 0.1f;
    GameModel model = Schedule.GetModel<GameModel>();

    public enum State
    {
        CharacterControl,
        DialogControl,
        Pause,

    }

    State state;

    public void ChangeState(State state) => this.state = state;

    void Update()
    {
        switch (state)
        {
            case State.CharacterControl:
                CharacterControl();
                break;
            case State.DialogControl:
                DialogControl();
                break;
            //case State.Pause:
            //    UIControls();
            //    break;

        }

        OpenUI();

    }

    void DialogControl()
    {
        model.player.nextMoveCommand = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            model.dialog.FocusButton(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            model.dialog.FocusButton(+1);
        if (Input.GetKeyDown(KeyCode.Space))
            model.dialog.SelectActiveButton();
    }

    void CharacterControl()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            model.player.nextMoveCommand = Vector3.up * stepSize;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            model.player.nextMoveCommand = Vector3.down * stepSize;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            model.player.nextMoveCommand = Vector3.left * stepSize;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            model.player.nextMoveCommand = Vector3.right * stepSize;
        else
            model.player.nextMoveCommand = Vector3.zero;

        InteractControl();
    }

    private void InteractControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (model.player.interactableObject != null)
                model.player.interactableObject.Interact();
        }
    }

    private void OpenUI()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            model.uiController.OpenCloseMenu();
        }
    }
}


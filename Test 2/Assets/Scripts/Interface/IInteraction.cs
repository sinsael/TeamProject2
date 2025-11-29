public interface IInteraction 
{
    public void OnInteract(PlayerInputHandler PlayerInput); // 상호작용 실행
    public void OnSelect(); // 상호작용 범위 진입
    public void OnDeselect(); // 상호작용 범위 이탈
}

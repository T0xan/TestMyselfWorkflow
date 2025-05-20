namespace T0xanGames.UnitsManagement
{
    public interface ISubordinate
    {
        public FollowPoint GetTargetPoint();
        public void SetTargetPoint(object sender, FollowPoint point);
    }
}

namespace TaskManagement.Repositiories
{
    public interface ITrackingState
    {
        void SetTrackingId(string trackingId);
        string? GetTrackingId();
    }
    public class TrackingState : ITrackingState
    {
        private string? _trackingId;
        public string? GetTrackingId()
        {
            return _trackingId;
        }

        public void SetTrackingId(string trackingId)
        {
            _trackingId = trackingId;
        }
      
}
}

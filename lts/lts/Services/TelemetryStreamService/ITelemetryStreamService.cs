namespace lts.Interfaces;
public interface ITelemetryStreamService
{
    bool StartStream(int partition);
    bool StopStream(int partition);
    bool IsStreamActive(int partition);
    IReadOnlyList<int> GetActiveStreams();
}

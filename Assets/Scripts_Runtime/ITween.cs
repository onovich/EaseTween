public interface ITween{
    void Play();
    void Pause();
    void Restart();
    void Tick(float dt);
    bool IsComplete { get; }
}
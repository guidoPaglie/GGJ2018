public enum PhoneCallState
{
    WAITING,
    PENDING,
    CONNECTED,
    FINISHED
}

public class PhoneCall
{
    public PhoneCall(int c, int r)
    {
        caller = c;
        receiver = r;

        state = PhoneCallState.WAITING;
    }

    public int caller;
    public int receiver;

    public PhoneCallState state;
}



public class Timer
{
    private bool b_Tricking;

    float f_Delay;
    //Current time
    private float f_CurTime;
    //Time to reach
    private float f_IntervalTime;

    private int i_CurCount;
    private int i_TriggerCount;


    //The trigger event list
    private event TimeManager.TimerDelegate tick;

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="second">Trigger Time</param>
    public Timer(float second , int count  ,float delay, TimeManager.TimerDelegate callback)
    {
        f_CurTime = -delay;  //投机取巧
        f_IntervalTime = second;

        i_CurCount = 0;
        i_TriggerCount = count;

        tick = callback;
        f_Delay = delay;

        b_Tricking = true;
    }


    /// <summary>
    /// Update Time
    /// </summary>
    public void Update(float deltaTime)
    {
        if (b_Tricking)
        {
            f_CurTime += deltaTime;

            if (i_CurCount < i_TriggerCount||i_TriggerCount<=0)
            {
                if (f_CurTime >= f_IntervalTime*i_CurCount)
                {
                    i_CurCount++;
                    tick(i_CurCount, f_Delay + f_CurTime);
                }
            }
            else
            {
                Stop();
            }
        }
      
    }



    /// <summary>
    /// Stop the Timer
    /// </summary>
    public void Stop()
    {
        b_Tricking = false;
    }

    public bool IsStop()
    {
        return !b_Tricking;
    }


}
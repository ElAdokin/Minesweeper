using UnityEngine;
using System.Collections.Generic;

public class Yielders
{
    private Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(100);
    private Dictionary<float, WaitForSecondsRealtime> _realTimeInterval = new Dictionary<float, WaitForSecondsRealtime>(100);

    private readonly WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();

    private WaitUntil _currentWaitUntil;


    public WaitForEndOfFrame EndOfFrame
    {
        get { return _endOfFrame; }
    }

    private readonly WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();

    public WaitForFixedUpdate FixedUpdate
    {
        get { return _fixedUpdate; }
    }

    public WaitForSeconds GetWaitForSeconds(float seconds)
    {
        if (!_timeInterval.ContainsKey(seconds))
            _timeInterval.Add(seconds, new WaitForSeconds(seconds));
        return _timeInterval[seconds];
    }

    public WaitForSecondsRealtime GetWaitForSecondsRealTime(float seconds)
    {
        if (!_realTimeInterval.ContainsKey(seconds))
            _realTimeInterval.Add(seconds, new WaitForSecondsRealtime(seconds));
        return _realTimeInterval[seconds];
    }

    public WaitUntil MyWaitUntil(System.Func<bool> predicate) 
    {
        _currentWaitUntil = new WaitUntil(predicate);
        return _currentWaitUntil;
    }
}

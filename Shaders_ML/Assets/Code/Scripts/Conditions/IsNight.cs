using Pada1.BBCore;           
using Pada1.BBCore.Framework; 
using Pada1.BBCore.Tasks;     
using UnityEngine;

[Condition("IsNight")]
public class IsNightCondition : ConditionBase 
{
    private DayNightCycle light;

    private bool SearchLight() 
    {
        if (light != null) return true;
    
        GameObject lightGO = GameObject.FindGameObjectWithTag("MainLight");
        if (lightGO == null) return false;

        light = lightGO.GetComponent<DayNightCycle>();
        return light != null;
    } 
    
    public override bool Check() 
    {
        return SearchLight() && light.IsNight;
    }

    public override TaskStatus MonitorCompleteWhenTrue() 
    {
        if (Check())  return TaskStatus.COMPLETED;
        
        if (light != null) light.OnChanged += OnSunset;
        
        return TaskStatus.SUSPENDED;
    }

    public override TaskStatus MonitorFailWhenFalse() 
    {
        if (!Check()) return TaskStatus.FAILED;
        
        light.OnChanged += OnSunrise;
        return TaskStatus.SUSPENDED;
    }

    public void OnSunset(object sender, System.EventArgs night) 
    {
        light.OnChanged -= OnSunset;
        EndMonitorWithSuccess();
    } 

    public void OnSunrise(object sender, System.EventArgs e) 
    {
        light.OnChanged -= OnSunrise;
        EndMonitorWithFailure();
    } 

    public override void OnAbort() 
    {
        if (SearchLight()) 
        {
            light.OnChanged -= OnSunrise;
            light.OnChanged -= OnSunset;
        }
        base.OnAbort();
    } 
} 
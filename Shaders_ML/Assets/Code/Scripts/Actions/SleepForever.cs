using Pada1.BBCore;           
using Pada1.BBCore.Framework; 
using Pada1.BBCore.Tasks;     

[Action("SleepForever")]
public class SleepForever : BasePrimitiveAction
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.SUSPENDED;
    }
}
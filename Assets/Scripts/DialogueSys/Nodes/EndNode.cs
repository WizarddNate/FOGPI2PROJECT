using UnityEngine;

[NodeTint("#732d28")]
public class EndNode : BaseNode
{
    [Input] public int entry;

    public override string GetString()
    {
        return "End";
    }
}

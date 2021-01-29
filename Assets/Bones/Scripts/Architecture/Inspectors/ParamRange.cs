using System;

namespace Bones.Scripts.Architecture.Inspectors
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParamRange : Attribute
    {
        public float min, max;

        public ParamRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
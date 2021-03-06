﻿using System;

namespace Jasper.Internals.Codegen
{
    public class StaticVariable : Variable, IVariableSource
    {
        public StaticVariable(Type variableType, string usage) : base(variableType, usage)
        {
        }

        public bool Matches(Type type)
        {
            return type == VariableType;
        }

        public Variable Create(Type type)
        {
            return this;
        }
    }
}
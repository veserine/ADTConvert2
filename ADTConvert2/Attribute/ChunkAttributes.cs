﻿using System;
using System.Runtime.CompilerServices;

namespace ADTConvert2.Attribute
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ChunkOrderAttribute : System.Attribute
    {
        private readonly int order_;
        public ChunkOrderAttribute([CallerLineNumber]int order = 0)
        {
            order_ = order;
        }

        public int Order { get { return order_; } }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ChunkOptionalAttribute : System.Attribute
    {
        private readonly bool optional_;
        public ChunkOptionalAttribute(bool optional = true)
        {
            optional_ = optional;
        }

        public bool Optional { get { return optional_; } }
    }
}

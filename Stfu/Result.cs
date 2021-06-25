//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2013—2021 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stfu
{
    public class Result<T>
        where T : new()
    {
        public Result(T val)
        {
            m_val = val;
        }

        public Result(T val, string message)
        {
            m_val = val;
            m_message = message;
        }

        public static implicit operator T(Result<T> val)
            => val.m_val;

        public static implicit operator Result<T>(ValueTuple<T, string> tuple)
            => new Result<T>(tuple.Item1, tuple.Item2);

        public string Message
            => m_message;

        private readonly T m_val;
        private readonly string m_message;
    }
}

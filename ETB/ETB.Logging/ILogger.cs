﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB.Logging
{
    public interface ILogger
    {
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, params object[] args);
        void Fatal(string message, params object[] args);
    }
}

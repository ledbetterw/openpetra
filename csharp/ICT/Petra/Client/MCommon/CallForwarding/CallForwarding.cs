//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2014 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MCommon.CallForwarding
{
    /// <summary>
    /// Sets up Delegates that allow arbitrary code to be called in various client-side DLLs,
    /// avoiding 'circular dependencies' between DLLs that need to call Methods in other DLLs.
    /// </summary>
    public class TCallForwarding
    {
        /// <summary>
        /// A static constructor is used to initialize any static data,
        /// or to perform a particular action that needs to be performed once only.
        /// It is called automatically before the first instance is created or any static members are referenced.Constructor.
        /// </summary>
        static TCallForwarding()
        {
            TClientTaskInstance.FastReportsPrintReportNoUiDelegate = FastReportsWrapper.PrintReportNoUi;
        }
    }
}
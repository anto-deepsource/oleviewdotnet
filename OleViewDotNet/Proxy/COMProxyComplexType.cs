﻿//    This file is part of OleViewDotNet.
//    Copyright (C) James Forshaw 2018
//
//    OleViewDotNet is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    OleViewDotNet is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with OleViewDotNet.  If not, see <http://www.gnu.org/licenses/>.

using NtApiDotNet.Ndr;
using OleViewDotNet.Utilities.Format;

namespace OleViewDotNet.Proxy;

public sealed class COMProxyComplexType : ICOMSourceCodeFormattable
{
    #region Public Properties
    public string Name => Entry.Name;

    public NdrComplexTypeReference Entry { get; }

    public int Size => Entry.GetSize();
    #endregion

    #region Internal Members
    internal COMProxyComplexType(NdrComplexTypeReference entry)
    {
        Entry = entry;
    }
    #endregion

    #region Public Methods
    void ICOMSourceCodeFormattable.Format(COMSourceCodeBuilder builder)
    {
        INdrFormatter formatter = builder.GetNdrFormatter();
        builder.AppendLine(formatter.FormatComplexType(Entry));
        builder.AppendLine();
    }
    #endregion
}
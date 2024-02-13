﻿//    This file is part of OleViewDotNet.
//    Copyright (C) James Forshaw 2014, 2016
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

using OleViewDotNet.Proxy;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace OleViewDotNet.TypeLib;

public sealed class COMTypeLibInterface : COMTypeLibInterfaceBase, IProxyFormatter
{
    #region Internal Members
    internal COMTypeLibInterface(COMTypeLibDocumentation doc, TYPEATTR attr) 
        : base(doc, attr)
    {
    }
    #endregion

    #region IProxyFormatter Implementation
    public string FormatText(ProxyFormatterFlags flags = ProxyFormatterFlags.None)
    {
        StringBuilder builder = new();
        bool is_dispatch = HasTypeFlag(TYPEFLAGS.TYPEFLAG_FDUAL);
        var base_interface = ImplementedInterfaces.FirstOrDefault();
        builder.AppendLine(GetTypeAttributes(true).FormatAttrs());
        int last_offset = base_interface?.Methods.LastOrDefault()?.VTableOffset ?? -1;
        if (base_interface == null)
        {
            builder.AppendLine($"interface {Name} {{");
        }
        else
        {
            builder.AppendLine($"interface {Name} : {base_interface.Name} {{");
        }
        foreach (var method in Methods.SkipWhile(m => m.VTableOffset <= last_offset))
        {
            string attrs = method.FormatAttributes(is_dispatch);
            if (!string.IsNullOrEmpty(attrs))
            {
                builder.Append("    ").AppendLine(attrs);
            }
            builder.Append("    ").AppendLine(method.FormatMethod());
        }
        builder.AppendLine("};");

        return builder.ToString();
    }
    #endregion
}

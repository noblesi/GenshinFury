// Copyright (c) 2013 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
// Licensed under the MIT License - https://raw.github.com/Robmaister/SharpNav/master/LICENSE

using System;

namespace SharpNav
{
	/// <summary>
	/// Determine which list the node is in.
	/// </summary>
	[Flags]
	public enum NodeFlags
	{
		/// <summary>
		/// Open list contains nodes to examine.
		/// </summary>
		Open = 0x01,

		/// <summary>
		/// Closed list stores path.
		/// </summary>
		Closed = 0x02
	}
}

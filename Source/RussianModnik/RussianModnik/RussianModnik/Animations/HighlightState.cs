﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RussianModnik.Animations
{
	class HighlightState
	{
		public int CurrHighlightedViewId { get; set; } = -1;

		public StrokeDash StrokeDash { get; set; }
		public HighlightPath HighlightPath { get; set; }
	}
}

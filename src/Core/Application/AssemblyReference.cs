﻿namespace Application;

using System.Reflection;

public class AssemblyReference
{
	public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
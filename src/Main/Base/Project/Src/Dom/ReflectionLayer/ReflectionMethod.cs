// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version value="$version"/>
// </file>
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace ICSharpCode.SharpDevelop.Dom
{
	[Serializable]
	public class ReflectionMethod : AbstractMethod 
	{
		MethodBase methodBase;
		
		
		public override bool IsConstructor {
			get {
				return methodBase is ConstructorInfo;
			}
		}

		public override IReturnType ReturnType {
			get {
				if (methodBase is MethodInfo) {
					return new ReflectionReturnType(((MethodInfo)methodBase).ReturnType);
				} 
				return null;
			}
			set {
			}
		}
		
		public override List<IParameter> Parameters {
			get {
				List<IParameter> parameters = new List<IParameter>();
				foreach (ParameterInfo paramInfo in methodBase.GetParameters()) {
					parameters.Add(new ReflectionParameter(paramInfo));
				}
				return parameters;
			}
			set {
			}
		}
		
		string GetParamList(MethodBase methodBase)
		{
			StringBuilder propertyName = new StringBuilder("(");
			ParameterInfo[] p = methodBase.GetParameters();
			if (p.Length == 0) {
				return String.Empty;
			}
			for (int i = 0; i < p.Length; ++i) {
				propertyName.Append(p[i].ParameterType.FullName);
				if (i + 1 < p.Length) {
					propertyName.Append(',');
				}
			}
			propertyName.Append(')');
			return propertyName.ToString();
		}
		
		public override string DocumentationTag {
			get {
				return "M:" + FullyQualifiedName + GetParamList(methodBase);
			}
		}
		
		public ReflectionMethod(MethodBase methodBase, IClass declaringType) : base(declaringType)
		{
			this.methodBase = methodBase;
			string name = methodBase.Name;
			
			if (methodBase is ConstructorInfo) {
				name = "#ctor";
			}
			FullyQualifiedName = methodBase.DeclaringType.FullName + "." + name;
			
			modifiers = ModifierEnum.None;
			if (methodBase.IsStatic) {
				modifiers |= ModifierEnum.Static;
			}
			if (methodBase.IsAssembly) {
				modifiers |= ModifierEnum.Internal;
			}
			if (methodBase.IsPrivate) { // I assume that private is used most and public last (at least should be)
				modifiers |= ModifierEnum.Private;
			} else if (methodBase.IsFamily) {
				modifiers |= ModifierEnum.Protected;
			} else if (methodBase.IsPublic) {
				modifiers |= ModifierEnum.Public;
			} else if (methodBase.IsFamilyOrAssembly) {
				modifiers |= ModifierEnum.ProtectedOrInternal;
			} else if (methodBase.IsFamilyAndAssembly) {
				modifiers |= ModifierEnum.Protected;
				modifiers |= ModifierEnum.Internal;
			}
			
			if (methodBase.IsVirtual) {
				modifiers |= ModifierEnum.Virtual;
			}
			if (methodBase.IsAbstract) {
				modifiers |= ModifierEnum.Abstract;
			}
		}
	}
}

using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Xml;
using System.Text;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Dom;

namespace ICSharpCode.Core
{
	public interface IProjectContent
	{
		XmlDoc XmlDoc {
			get;
		}
		ICollection<IClass> Classes {
			get;
		}
		
		string GetXmlDocumentation(string memberTag);
		
		Hashtable AddClassToNamespaceList(IClass addClass);
		void UpdateCompilationUnit(ICompilationUnit oldUnit, ICompilationUnit parserOutput, string fileName, bool updateCommentTags);
		
		IClass GetClass(string typeName);
		bool NamespaceExists(string name);
		string[] GetNamespaceList(string subNameSpace);
		ArrayList GetNamespaceContents(string subNameSpace);
		
		string SearchNamespace(string name, ICompilationUnit unit, int caretLine, int caretColumn);
		IClass SearchType(string name, IClass curType, int caretLine, int caretColumn);
		IClass SearchType(string name, IClass curType, ICompilationUnit unit, int caretLine, int caretColumn);
		
		bool IsAccessible(IClass c, IDecoration member, IClass callingClass, bool isClassInInheritanceTree);
		bool MustBeShown(IClass c, IDecoration member, IClass callingClass, bool showStatic, bool isClassInInheritanceTree);
		
		ArrayList ListTypes(ArrayList types, IClass curType, IClass callingClass);
		ArrayList ListMembers(ArrayList members, IClass curType, IClass callingClass, bool showStatic);
		
		Position GetPosition(string fullMemberName);
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;


namespace robotlegs.bender.extensions.viewManager.api
{
	public interface IViewManager
	{
		void AddContainer(object container);
		void AddViewHandler(IViewHandler handler);
		void RemoveViewHandler(IViewHandler handler);
		void RemoveAllHandlers();
		event Action<object> ContainerAdded;
		event Action<object> ContainerRemoved;
		List<object> Containers { get; }
	}
}


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
using robotlegs.bender.extensions.viewManager;
using UnityEngine;
using System.Collections.Generic;
using robotlegs.bender.extensions.viewManager.impl;


namespace robotlegs.bender.framework.unity.extensions.viewManager.impl
{
	public class UnityParentFinder : IParentFinder
	{
		// Container contains container
		public bool Contains(object parentContainer, object childContainer)
		{
#if UNITY_EDITOR
			// Check only in the editor the user is using transforms
			if ((parentContainer != null && !(parentContainer is Transform)) || (childContainer != null && !(childContainer is Transform)))
				throw new Exception("Container must always be a transform");
#endif

			Transform parentTransform = parentContainer as Transform;
			Transform childTransform = childContainer as Transform;

			// No child to be contained
			if (childTransform == null)
				return false;
			// Container is the stage/root
			if (parentTransform == null)
				return true;

			while (childTransform != null)
			{
				if (childTransform.parent == parentTransform)
					return true;

				childTransform = childTransform.parent;
			}
			return false;
		}

		// View Find Parent Container
		public object FindParent(object childView, Dictionary<object, ContainerBinding> containers)
		{
			return FindParent (childView, new List<ContainerBinding>(containers.Values));
		}

		public object FindParent(object childView, List<ContainerBinding> containers)
		{
			if (childView is Component)
			{
				childView = (childView as Component).transform;
			}
			else if (childView is GameObject)
			{
				childView = (childView as GameObject).transform;
			}
			else
				return null;

			Transform transform = childView as Transform;
			while (transform != null)
			{
				foreach (ContainerBinding containerBinding in containers)
				{
					if (containerBinding.Container is Transform && (Transform)containerBinding.Container == transform.parent)
						return containerBinding.Container;
				}
				transform = transform.parent;
			}

			return null;
		}
	}
}


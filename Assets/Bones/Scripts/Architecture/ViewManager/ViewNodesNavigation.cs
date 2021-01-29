using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using XNode;

namespace Bones.Scripts.Architecture.ViewManager
{
	[CreateAssetMenu(menuName = "Bones/View Nodes Navigation")]
	public class ViewNodesNavigation : NodeGraph
	{
		
		[Serializable]
		public struct PrefabWithNode 
		{
			public ViewNode prefab;
			public ViewNodeVisualization node;
		}
		
		
		public List<ViewNode> elements;
		
		private void OnValidate()
		{
			UpdateModel();
		}

		private Dictionary<Type, ViewNodeVisualization> nodeToView = new Dictionary<Type, ViewNodeVisualization>();
		
		public void Init()
		{
			nodeToView = nodes
				.OfType<ViewNodeVisualization>()
				.ToDictionary(
					n => n.prefab.GetType(), 
					n => n);
		}



		public ViewNode FromNode(Type type, string outputKey)
		{
			if (nodeToView.TryGetValue(type, out var nodeSource))
			{
				for (var i = 0; i < nodeSource.outputs.Length; i++)
				{
					var t = nodeSource.outputs[i];
					if (string.Equals(t, outputKey, StringComparison.CurrentCultureIgnoreCase))
					{
						var port = nodeSource.GetPort("outputs " + i);
						if (port.Connection != null && port.Connection.node != null)
						{
							if (port.Connection.node is ViewNodeVisualization nodeDestination)
							{
								return nodeDestination.prefab;
							}
						}
					}
				}
			}
			return null;
		}
		
	
		public T Find<T>() where T : ViewNode
		{
			return elements.OfType<T>().First();
		}

		public ViewNode Find(Type type)
		{
			return elements.First(t => t.GetType() == type);
		}

		public void ValidateNode(ViewNodeVisualization node)
		{
			if (node.prefab && !elements.Contains(node.prefab))
			{
				elements.Add(node.prefab);
			}

			UpdateModel();
		}

		private void UpdateModel()
		{
			var viewNodes = nodes.OfType<ViewNodeVisualization>().Select(n=>n.prefab).ToList();
			for (var i = 0; i < elements.Count; i++)
			{
				if (!viewNodes.Contains(elements[i]))
				{
					elements.RemoveAt(i);
				}
			}
		}

		public void AddAll(List<ViewNode> prefabs, Vector2 viewPosition, Vector2 nodeSize)
		{
			Vector2 pos = viewPosition;

			foreach (var viewNode in prefabs)
			{
				
				var node = AddNode<ViewNodeVisualization>();
				node.prefab = viewNode;
				node.name = viewNode.GetType().Name;
				node.position = pos;
				pos += Vector2.right * nodeSize.x * .5f;
			}
		}
	}
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace KahuInteractive.HassleFreeConfig
{

    public class ConfigTree
    {
        private int _idIterator;
        private List<TreeViewItemData<IConfigValueOrGroup>> _data;

        public ConfigTree(ConfigValueGroup root)
        {
            _idIterator = 0;

            _data = new List<TreeViewItemData<IConfigValueOrGroup>>();
            _data.Add(ConvertToTreeViewData(root));
        }


        private TreeViewItemData<IConfigValueOrGroup> ConvertToTreeViewData(ConfigValue value)
        {
            return new TreeViewItemData<IConfigValueOrGroup>(_idIterator++, value, null);
        }

        private TreeViewItemData<IConfigValueOrGroup> ConvertToTreeViewData(ConfigValueGroup value)
        {
            List<TreeViewItemData<IConfigValueOrGroup>> childrenElements = new ();

            // Get child elements here....
            foreach (ConfigValueGroup childGroup in value.GetSubGroups())
            {
                childrenElements.Add(ConvertToTreeViewData(childGroup));
            }
            foreach (ConfigValue childValue in value.GetValues())
            {
                childrenElements.Add(ConvertToTreeViewData(childValue));
            }

            return new TreeViewItemData<IConfigValueOrGroup>(_idIterator++, value, childrenElements);
        }


        public IList<TreeViewItemData<IConfigValueOrGroup>> Get()
        {
            return _data;
        }
    }


}


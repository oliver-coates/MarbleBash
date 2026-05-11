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
            _data.Add(ConvertToTreeViewData(root, 0));
        }


        private TreeViewItemData<IConfigValueOrGroup> ConvertToTreeViewData(ConfigValue value, int id)
        {
            return new TreeViewItemData<IConfigValueOrGroup>(id, value, null);
        }

        private TreeViewItemData<IConfigValueOrGroup> ConvertToTreeViewData(ConfigValueGroup value, int id)
        {
            List<TreeViewItemData<IConfigValueOrGroup>> childrenElements = new ();
            int thisId = id;
            id++;

            // Get child elements here....
            foreach (ConfigValueGroup childGroup in value.GetSubGroups())
            {
                childrenElements.Add(ConvertToTreeViewData(childGroup, id));
            }
            foreach (ConfigValue childValue in value.GetValues())
            {
                childrenElements.Add(ConvertToTreeViewData(childValue, id));
                id++;                
            }

            return new TreeViewItemData<IConfigValueOrGroup>(thisId, value, childrenElements);
        }


        public IList<TreeViewItemData<IConfigValueOrGroup>> Get()
        {
            return _data;
        }
    }


}


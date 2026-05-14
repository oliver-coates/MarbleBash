using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace KahuInteractive.HassleFreeConfig
{

    public class ConfigTree
    {
        private int _idIterator;
        private List<TreeViewItemData<IConfigValueOrGroup>> _data;
        private Dictionary<IConfigValueOrGroup, int> _configValueIdDict;

        public ConfigTree(ConfigValueGroup root)
        {
            _idIterator = 0;

            _configValueIdDict = new Dictionary<IConfigValueOrGroup, int>();
            _data = new List<TreeViewItemData<IConfigValueOrGroup>>();
            _data.Add(ConvertToTreeViewData(root));
        }


        private TreeViewItemData<IConfigValueOrGroup> ConvertToTreeViewData(ConfigValue value)
        {
            _configValueIdDict.Add(value, _idIterator);
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

            _configValueIdDict.Add(value, _idIterator);
            return new TreeViewItemData<IConfigValueOrGroup>(_idIterator++, value, childrenElements);
        }


        public IList<TreeViewItemData<IConfigValueOrGroup>> Get()
        {
            return _data;
        }

        public int GetElementId(IConfigValueOrGroup toGet)
        {
            try
            {
                int id = _configValueIdDict[toGet];
                return id;              
            }
            catch
            {
                // Debug.LogError(e);
                // Debug.LogError($"Could not find key: {toGet.GetName()}");
                return 0;
            }

            
        }
    }


}


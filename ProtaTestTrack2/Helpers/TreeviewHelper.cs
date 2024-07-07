using MudBlazor;
using ProtaTestTrack2.Model;
namespace ProtaTestTrack2.Helpers;

public class TreeItemData
{
    public TreeItemData(string title, string valuex, string icon, int? number = null)
    {
    Title = title;
    Value = valuex;
    Icon = icon;
    Number = number;
    IsExpanded = false;
    TreeItems = new HashSet<TreeItemData>();
    }

    public string Title { get; set; }
    
    public string Value { get; set; }

    public string Icon { get; set; }

    public int? Number { get; set; }

    public bool IsExpanded { get; set; }

    public HashSet<TreeItemData> TreeItems { get; set; }
}
public class TreeviewHelper
{
    private readonly IEnumerable<Feature> features;
    public HashSet<TreeItemData> TreeItems { get; set; }
    public TreeviewHelper(IEnumerable<Feature> features)
    {
    this.features = features;
    }
    public void BuildTree()
    {
    if (features != null)
    {
    TreeItems = new HashSet<TreeItemData>();

            var subItems = features.Where(item => item.ParentFeatureID == null);
            foreach (var item in subItems)
            {
                BuildTreeItem(item, null);
            }
        }
    }

    private void BuildTreeItem(Feature root, TreeItemData? data)
    {
        if (features != null)
        {
            var treeItem = new TreeItemData(root.Name ?? string.Empty, root.FeatureID, Icons.Material.Filled.AccountCircle);
            var subItems = features.Where(item => item.ParentFeatureID == root.FeatureID);

            foreach (var subItem in subItems)
            {
                BuildTreeItem(subItem, treeItem);
            }

            if (data is null)
            {
                TreeItems.Add(treeItem);
            }
            else
            {
                data.TreeItems.Add(treeItem);
            }

        }
    }
}
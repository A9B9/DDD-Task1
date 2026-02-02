using WebApplication3.Domain.Entities.products;

namespace WebApplication3.Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool IsActive { get; private set; } = true;

    // Navigation Properties 
    public Guid ? ParentId { get; private set; }
    public Category ?  Parent { get; private set; }
    public List<Category> Children { get; private set; } = [];
    public List<Product> Products { get; private set; } = [];

    #region Domain Methods

    public void SetParent(Category newParent)
    {
        if(newParent == null)
            throw new InvalidOperationException("Parent cannot be null");
        if(newParent == this)
            throw new InvalidOperationException("Parent cannot be the same");
        var currentParent = newParent;
        while (currentParent != null)
        {
            if(currentParent == this)
                throw new InvalidOperationException("Parent cannot be the same");
            currentParent = currentParent.Parent;
        }
        this.Parent = newParent;
        this.ParentId = newParent.ParentId;
    }

    public void AddChild(Category newChild)
    {
        if(this.Children.FirstOrDefault(x => x.Id == newChild.Id) == null)
            throw new InvalidOperationException("Child doesn't exist");
        if (newChild.Parent != null)
            throw new InvalidOperationException("Parent already exists");
        
        newChild.Parent = this;
        newChild.ParentId = this.ParentId;
        this.Children.Add(newChild);
            
    }

    public void RemoveChild(Category child)
    {
        if(this.Children.FirstOrDefault(x => x.Id == child.Id) == null)
            throw new InvalidOperationException("Child doesn't exist");
        child.Parent = null;
        child.ParentId = null;
        this.Children.Remove(child);
    }

    public void Activate() => this.IsActive = true;
    
    public void Deactivate() => this.IsActive = false;
    
    #endregion

}
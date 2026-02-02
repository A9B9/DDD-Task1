using System.Diagnostics;
using WebApplication3.Domain.Entities.Inventories;
using WebApplication3.Domain.Enums;

namespace WebApplication3.Domain.Entities.products;

public class Product : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Sku { get; set; }

    public ProductStatus Status { get; private set; }
    public ICollection<ProductStatusHistory> StatusHistory { get; set; } = [];
    public ICollection<PriceHistory> PriceHistory { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
    public ICollection<ProductAttachment> Attachments { get; set; } = [];

    // Navigation 
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public Inventory Inventory { get; private set; }

    #region Domain Methods

    public void ChangePrice(decimal newPrice)
    {
        switch (this.Status)
        {
            case ProductStatus.Deleted:
            case ProductStatus.Archived:
            case ProductStatus.Draft:
            case ProductStatus.Inactive:
                throw new InvalidOperationException("Cannot change the prince of a draft, or inactive product");
            default:
                this.Price = newPrice;
                var priceHistory = new PriceHistory()
                {
                    Price = newPrice,
                    Product = this,
                    ProductId = this.Id
                };
                PriceHistory.Add(priceHistory);
                break;
        }

    }

    public void Activate()
    {
        this.Status = this.Status switch
        {
            ProductStatus.Deleted or ProductStatus.Published => throw new InvalidOperationException(
                "Cannot activate a deleted or published product"),
            _ => ProductStatus.Active
        };
    }

    public void Deactivate()
    {
        this.Status = this.Status switch
        {
            ProductStatus.Deleted or ProductStatus.Draft => throw new InvalidOperationException(
                "Cannot Deactivate a deleted or draft product "),
            _ => ProductStatus.Inactive,
        };
    }

    public void Delete()
    {
        this.Status = this.Status switch
        {
            ProductStatus.Published or ProductStatus.Active => throw new InvalidOperationException(
                "Can't Delete a published or active product, deactivate first"),
            _ => ProductStatus.Deleted
        };
    }

    public void Publish()
    {
        this.Status = this.Status switch
        {
            ProductStatus.Published or ProductStatus.Deleted or ProductStatus.Archived or ProductStatus.Inactive =>
                throw new InvalidOperationException(
                    "Can't Publish a published or active product"),
            _ => ProductStatus.Published
        };
    }

    public void Archive()
    {
        this.Status = this.Status switch
        {
            ProductStatus.Archived => throw new InvalidOperationException("Product already archived!"),
            ProductStatus.Deleted => throw new InvalidOperationException("Product already deleted!"),
            ProductStatus.Published => throw new InvalidOperationException(
                "Can't Archive a published product!, you need to deactivate it first"),
            _ => ProductStatus.Archived
        };
    }

    public void AddTag(Tag newTag)
    {
        if (this.Tags.FirstOrDefault(t => t.Id == newTag.Id) != null)
            throw new InvalidOperationException("Tag already exists!");
        this.Tags.Add(newTag);
    }

    public void RemoveTag(Tag tag)
    {
        if (this.Tags.FirstOrDefault(t => t.Id == tag.Id) == null)
            throw new InvalidOperationException("Tag doesn't exists!");
        this.Tags.Remove(tag);
    }

    public void AddAttachment(ProductAttachment newAttachment)
    {
        if (this.Attachments.FirstOrDefault(a => a.Id == newAttachment.Id) != null)
            throw new InvalidOperationException("Attachment already exists!");
        this.Attachments.Add(newAttachment);
    }

    public void RemoveAttachment(ProductAttachment attachment)
    {
        if (this.Attachments.FirstOrDefault(a => a.Id == attachment.Id) == null)
            throw new InvalidOperationException("Attachment doesn't exists!");
        this.Attachments.Remove(attachment);
    }

    public void AssignCategory(Category newCategory)
    {
        if (this.CategoryId == newCategory.Id)
            throw new InvalidOperationException("Category already assigned!");
        this.Category = newCategory;
        this.CategoryId = newCategory.Id;
    }

    #endregion
}
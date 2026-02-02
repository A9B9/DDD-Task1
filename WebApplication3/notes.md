## Main Models

- **Category:**
    - Name
    - Ref Products (one to many)
    - Description
    - Icon (opt)
    - IsDeleted ?
    - Children (one to many)
    - Ref Parent (many to one)
    - Created At
    - Updated At

- **Product:**
    - Name
    - Description
    - Cost
    - Current Price
    - Ref (Price history)
    - SKU
    - Qty
    - Image / Images
    - Ref Category
    - Ref Tags
    - IsPublished ?
    - IsDeleted ?
    - InStock ?
    - Status (Active, Draft, Archived, Disabled)
    - Ref StatusHistory
    - Created At
    - Updated At

- **Tag**
    - Name
    - Color
    - Products (Navigation)

- **Inventory:**
    - Ref (Product)
    - QrCode
    - IsDeleted ?
    - Stock Level
    - Reserved Stock
    - Threshold
    - Created At
    - Updated At

- **Order**
    - Ref User
    - Status
    - Ref Items
    - Ref Timeline
    - IsDeleted ?
    - Created At
    - Updated At 


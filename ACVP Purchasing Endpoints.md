# ACVTS Purchasing Endpoints

This document outlines the various endpoints around purchasing and tracking vector set usage. These endpoints are available on both Demo and Prod, but some underlying actions will only be triggered on Prod. Purchasing vector sets is not available on Demo. Starting October 1, 2020, all Production users are *REQUIRED* to purchase before use. A grace period starting October 1, 2020 until 11:59pm EDT November 30, 2020 allows users in Production to use vector sets before purchasing before the grace period expires. 

Purchases are made by an accredited 17ACVT lab with a signed CRADA for the current FY. All users in Production belong to a lab. The lab is the entity that owns any purchased vector sets. All users that belong to the lab share the same pool of available vector sets. 

## 1. Purchase Options

The following endpoint and response outline the options available for purchase. Options and prices in examples are for the current fiscal year (FY).  Prices and amounts may change from FY to FY.  An FY runs from October 01 of one year to September 30 of the following year.

```
GET /purchase/options
```

Returns a listing of all current valid options for purchasing vector sets.

### 1.1 Response

```
{
    {
        "acvVersion": "1.0"
    },
    {
        "data": [
            {
                "url": "/acvp/v1/purchase/options/1",
                "name": "100 vector sets",
                "description": "100 vector sets, no expiration",
                "price": "1000.00 USD"
            },
            {
                "url": "/acvp/v1/purchase/options/2",
                "name": "500 vector sets",
                "description": "500 vector sets, no expiration",
                "price": "4000.00 USD"
            },
            {
                "url": "/acvp/v1/purchase/options/3",
                "name": "Unlimited subscription during FY2021",
                "description": "Unlimited vector sets during FY2021",
                "price": "10000.00 USD"
            },
        ]
    }
}
```

## 2. Purchase

Purchasing a vector set is a multi-phase operation. Only one request to the API is necessary. Out of band, NIST will prepare an invoice which will be sent to the 17ACVT lab that made the request. The lab is responsible for payment according to the invoice. When this has been processed by NIST, the available vector sets for the lab will be updated. Processing may take a couple of days. If issues arise, please contact the CAVP Program Manager. 

```
POST /purchase
```

Initiates a purchase of vector sets. Multiple purchase options, and multiples of each, may be included in a single purchase in order to provide the desired total number of vector sets. If an unlimited option is included the quantity must be one, and there may be no other items included in the purchase.

### 2.1 Request

```
{
    {
        "acvVersion": "1.0"
    },
    {
        "items": [
            { 
                "purchaseOptionUrl": "/acvp/v1/purchase/options/2",
                "quantity": 3
            },
            { 
                "purchaseOptionUrl": "/acvp/v1/purchase/options/1",
                "quantity": 1
            }
        ]
    }
}
```

### 2.2 Response

```
{
    {
        "acvVersion": "1.0"
    },
    {
        "message": "Purchase initiated"
    }
}
```

## 3. Available Vector Sets

```
GET /lab/availablevectorsets
```

Returns how many vector sets your lab has for use based on purchases for which payment has been received and previous vector set usage. Value will either be a number of vector sets, or "unlimited"

### 3.1 Response

```
{
    {
        "acvVersion": "1.0"
    },
    {
        "available": "100"
    }
}
```

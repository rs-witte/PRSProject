## **Purchase Request System (PRS) - Web Services**
The purpose of this document is to provide a detailed description of RESTful web services associated with the Purchase Request System (PRS). Web services associated with each controller have been listed in their respective sections along with additional information:

**Important Reminders:**
-  **404 Error:** Returned when information entered in route does not correspond to a record on associated PRSDB database table. Not found.
- **Action Not Permitted:** `Id` column data on PRSDB database tables cannot be updated/modified via PUT. _DO NOT_ attempt to manually update `Id` on database tables as this can result in corrupted data.
- **RecalculateRequestTotal(requestId):** This private method is executed asynchronously and recalculates the `Total` property on the **`Requests`** database table whenever an insert, update, or delete occurs to the request lines associated with the request (`Id` on **`Requests`**). This method is called from the PUT, POST, and DELETE methods _AFTER_ SaveChangesAsync() is called in those methods. See "RequestLineController.cs" code. 


## User Controller
Web services below are associated with the User controller and **`Users`** table created within the PRSDB database. Additional details have been provided beneath the figure below using the corresponding number from the figure table:

| \# | Type   |  Route                                  | Short Description                 |
|:---| :---   | :----                                   | :---                              |
| 1  | GET    | /api/users                              | List - ALL Users                  |
| 2  | GET    | /api/users/_\<id>_                      | Search - By User ID               |
| 3  | GET    | /api/users/username/_\<username>_       | Search - By Username              |
| 4  | GET    | /api/users/_\<username>_/_\<password>_  | Authentication (User Credentials) |
| 5  | POST   | /api/users/                             | Create - New User                 |
| 6  | PUT    | /api/users/_\<id>_                      | Update - Existing User            |
| 7  | DELETE | /api/users/_\<id>_                      | Delete - Existing User            |

**Additional Information:**
1. Returns a list of ALL user records and information represented on the **`Users`** table
2. Returns ALL information associated with a _single_ specified user, searched for by `Id`
3. Returns ALL information associated with a _single_ specified user, searched for by `Username`
4. Validates user credentials by returning a _single_ user's information, searched for by entering a specific user's `Username` and `Password` combination into the route. Combination must match exactly for a result to be returned.
5. Create/Add a new user to the **`Users`** table
6. Update information for a _single_ specified user, must use valid `Id` in route
7. Delete a _single_ specified user, must use valid `Id` in route. Record on **`Users`** table will be deleted. 


## Vendor Controller:
Web services below are associated with the Vendor controller and **`Vendors`** table created within the PRSDB database. Additional details have been provided beneath the figure below using the corresponding number from the figure table:

| \# | Type    |  Route                 | Short Description           |
|:---| :---    | :----                  | :---                        |
| 1  | GET     | /api/vendors           | List - ALL Vendors          |
| 2  | GET     | /api/vendors/_\<id>_   | Search - By Vendor ID       |
| 3  | POST    | /api/vendors/          | Create - New Vendor         |
| 4  | PUT     | /api/vendors/_\<id>_   | Update - Existing Vendor    |
| 5  | DELETE  | /api/vendors/_\<id>_   | Delete - Existing Vendor    |

**Additional Information:**
1. Returns a list of ALL vendor records and information represented on the **`Vendors`** table
2. Returns ALL information associated with a _single_ specified vendor, searched for by `Id`
3. Create/Add a new vendor to the **`Vendors`** table
4. Update information for a _single_ specified vendor, must use valid `Id` in route
5. Delete a _single_ specified vendor, must use valid `Id` in route. Record on **`Vendors`** table will be deleted. 


## Product Controller:
Web services below are associated with the Product controller and **`Products`** table created within the PRSDB database. Additional details have been provided beneath the figure below using the corresponding number from the figure table:

| \# | Type    |  Route                 | Description                 |
|:---| :---    | :----                  | :---                        |
| 1  | GET     | /api/products          | List - ALL Products         |
| 2  | GET     | /api/products/_\<id>_  | Search - By Product ID      |
| 3  | POST    | /api/products/         | Create - New Product        |
| 4  | PUT     | /api/products/_\<id>_  | Update - Existing Product   |
| 5  | DELETE  | /api/products/_\<id>_  | Delete - Existing Product   |

**Additional Information:**
1. Returns a list of ALL product records and information represented on the **`Products`** table
2. Returns ALL information associated with a _single_ specified product, searched for by `Id`
3. Create/Add a new product to the **`Products`** table
4. Update information for a _single_ specified product, must use valid `Id` in route
5. Delete a _single_ specified product, must use valid `Id` in route. Record on **`Products`** table will be deleted. 


## Request Controller:
Web services below are associated with the Request controller and **`Requests`** table created within the PRSDB database. Additional details have been provided beneath the figure below using the corresponding number from the figure table:

| \# | Type   | Route                             | Description                        |
|:---| :---   | :----                             | :---                               |
| 1  | GET    | /api/requests                     | List - ALL Requests                |
| 2  | GET    | /api/requests/_\<id>_             | Search - By Request ID             |
| 3  | GET    | /api/requests/review/_\<UserId>_  | List - Review Status Requests      |
| 4  | POST   | /api/requests/                    | Create - New Request               |
| 5  | PUT    | /api/requests/_\<id>_             | Update - Existing Request          |
| 6  | PUT    | /api/requests/_\<id>_/review      | Update - Request Status (REVIEW)   |
| 7  | PUT    | /api/requests/_\<id>_/approve     | Update - Request Status (APPROVED) |
| 8  | PUT    | /api/requests/_\<id>_/reject      | Update - Request Status (REJECTED) |
| 9  | DELETE | /api/requests/_\<id>_             | Delete - Existing Request          |

**Additional Information:**
1. Returns a list of ALL request records represented on the **`Requests`** table
2. Returns ALL information associated with a _single_ specified request, searched for by `Id`
3. Returns a list of ALL request records with a `Status` of "REVIEW", must use valid `UserID` in route
    - List _EXCLUDES_ request records associated with the `UserID` in the route 
4. Create/Add a new request to the **`Requests`** table
5. Update information for a _single_ specified request, must use valid `Id` in route
6. Update existing request's `Status` to "REVIEW", must use valid `Id` in route
    - _IF request `Total` > $50 the request's `Status` will change to "REVIEW"_
    - _IF request `Total` <= $50 the request's `Status` will automatically change to "APPROVED"_
7. Change existing request's `Status` to "APPROVED", must use valid `Id` in route 
8. Change existing request's `Status` to "REJECTED",must use valid `Id` in route
9. Delete a _single_ specified request, must use valid `Id` in route. Record on **`Requests`** table will be deleted. 


## RequestLine Controller:


## Notes:
* Review the Word document and a make a list of needed RESTful web services and include this in your documentation. Something like this:

    **API Requirements for User:**

        GET  /api/users       all users
        GET  /api/users/123   one user or 404
        GET  /api/user/username/password  one user or 404 return at least name, IsAdmin, IsReviewer and id
        POST /api/user
        PUT  /api/user/12

* While the specs mention "virtual" methods, you do not need to create them as virtual.
* Do not return more data than needed. Always consider security!

# Rental4You

PWEB Project 2022/2023 - Web Application for managing car rentals.

## Areas of the Website

### Free access

**Access (minimum role): Unauthorized Users**

- [ ] Search vehicles by location, vehicle type and date (pickup and delivery date)
- [ ] Display list of available vehicles after search with cost and rental company
- [ ] Filter results by vehicle category and company
- [ ] Sort results by lowest/highest price and/or by company rating
- [ ] Register as a Customer

### Customer Area

**Access (minimum role): Registered Users with a Profile**

- [ ] Search for vehicles (like in free access area)
- [ ] Book a vehicle
- [ ] Consult the reservation history
- [ ] Consult details of a reservation

### Employee Area

**Access (minimum role): Users with an Employee profile and associated with a company**

#### Manage the vehicle fleet of the company

- [x] List vehicle registrations - with filters (category, status) and sorted
- [x] Register new vehicle
- [x] Edit vehicle registration
- [x] Delete vehicle registration (only if there are no reservations for this vehicle)
- [x] Activate or deactivate vehicle registration

#### Manage company reservations

- [x] List vehicle reservations (with filters: pickup date, delivery date, category, vehicle, customer)
- [ ] Confirm a Reservation
- [ ] Reject a Reservation
- [ ] Deliver a vehicle to customer (pick up by customer)
- [ ] Status of delivery
- [ ] Vehicle kilometres
- [ ] Vehicle damage (yes/no)
- [ ] Remarks
- [ ] Employee who made the delivery
- [ ] Receiving a vehicle from the customer (delivery by customer)
- [ ] Indicate the condition in which the vehicle is delivered by the customer
  - Kilometres
  - Damage (yes/no)
  - If damaged: photos must be attached to document the damage
  - Employee who received the vehicle
  - Notes

### Manager Area

**Access (minimum role): Users with a Manager profile and associated with a company**

#### Employee Management

- [x] Register new user as an Employee
- [x] Register new user as a Manager
- [x] List registered users of the company
- [x] Enable or disable user registration
- [x] Delete user registration (if not associated with any reservation)
- [x] It is not possible to delete the own registration
- [x] Manage reservations (same as Employee profile)
- [x] Manage vehicle fleet (same as Employee profile)

#### Dashboard (optional)

- [ ] Invoicing amount of last 7 days
- [ ] Invoicing amount of last 30 days
- [ ] Average number of daily reservations in the last 30 days
- [ ] Graph with number of reservations per day (last 30 days)

### Platform administrator area

**Access (minimum role): Only Admins of the Website**

#### Business Management

- [x] List companies - with filters (name, subscription status) and with ordering
- [x] Register new company - automatically creates a new user associated with the company (role: manager)
- [x] Edit company registration
- [x] Delete company registration (only if there are no vehicles yet)
- [x] Activate or deactivate companies
- [x] Vehicle category management - list, create, edit, delete
- [x] User management - list, edit (activate/deactivate)

#### Dashboard with information about booking performance (optional)

- [ ] Number of daily bookings in the last 30 days
- [ ] Number of monthly bookings (last 12 months)
- [ ] Number of new customers per month (last 12 months)

## Models (Database Tables)

**User**

- Id
- UserName (Login)
- Name
- RoleId
- CompanyId

**Role**

- Id
- Name (Customer, Employee, Manager, Admin)

**Company**

- Id
- Name
- Active
- Rating

**Vehicle**

- Id
- VehicleCategoryId
- CompanyId
- Kilometres
- Active
- Location
- Cost

**VehicleCategory**

- Id
- Name
- List<Vehicle>

**Reservation**

- Id
- VehicleId
- CustomerId
- CreatedDate
- Status (open, confirmed, rejected, closed)
- PickupId
- DeliveryId

**Pickup**

- Id
- EmployeeId
- PickupDate
- Kilometres
- Damage
- Remarks

**Delivery**

- Id
- EmployeeId
- DeliveryDate
- Kilometres
- Damage
- List<Photo>
- Remarks

**DeliveryPhoto**

- Id
- FilePath
- DeliveryId

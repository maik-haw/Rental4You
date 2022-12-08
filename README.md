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
- [ ] List vehicle registrations - with filters (category, status) and sorted
- [ ] Register new vehicle
- [ ] Edit vehicle registration
- [ ] Delete vehicle registration (only if there are no reservations for this vehicle)
- [ ] Activate or deactivate vehicle registration

#### Manage company reservations
- [ ] List vehicle reservations (with filters: pickup date, delivery date, category, vehicle, customer)
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
- [ ] Register new user as an Employee
- [ ] Register new user as a Manager
- [ ] List registered users of the company
- [ ] Enable or disable user registration
- [ ] Delete user registration (if not associated with any reservation)
- [ ] It is not possible to delete the own registration
- [ ] Manage reservations (same as Employee profile)
- [ ] Manage vehicle fleet (same as Employee profile)

#### Dashboard (optional)
- [ ] Invoicing amount of last 7 days
- [ ] Invoicing amount of last 30 days
- [ ] Average number of daily reservations in the last 30 days
- [ ] Graph with number of reservations per day (last 30 days)

### Platform administrator area
**Access (minimum role): Only Admins of the Website**

#### Business Management
- [ ] List companies - with filters (name, subscription status) and with ordering
- [ ] Register new company - automatically creates a new user associated with the company (role: manager)
- [ ] Edit company registration
- [ ] Delete company registration (only if there are no vehicles yet)
- [ ] Activate or deactivate companies
- [ ] Vehicle category management - list, create, edit, delete
- [ ] User management - list, create, edit, delete

#### Dashboard with information about booking performance
- [ ] Number of daily bookings in the last 30 days
- [ ] Number of monthly bookings (last 12 months)
- [ ] Number of new customers per month (last 12 months)

## User Types (Roles)
- Anonymous
- Customer
- Employee
- Manager
- Administrator